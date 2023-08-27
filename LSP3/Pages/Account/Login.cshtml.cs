using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Text.Json;

using static System.Reflection.Metadata.BlobBuilder;

namespace LSP3.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string? Username { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        static readonly char[] padding = { '=' };

        public LoginModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPost()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var author = OnGetauthor(Username, Password);

                _httpContextAccessor.HttpContext.Session.SetString("Authenticated", "true");
                _httpContextAccessor.HttpContext.Session.SetString("currentuser", Username);
            }
            await OnGetauthor(Username, Password);

            return RedirectToPage("/Index");
        }

        public async Task OnGetauthor(string? username, string? password)
        {
            AuthorDto author = new AuthorDto();

            if (username == null || password == null) return;

            try
            {
                byte[]? arrencrypted = await new EncryptionService().EncryptAsync(password);
                var encrypted = Convert.ToBase64String(arrencrypted).TrimEnd(padding).Replace('+', '-').Replace('/', '_');


                string apiResponse = await new HttpHelper().Get($"https://localhost:7253/api/author/{username}/{encrypted}");
                if (apiResponse != null)
                {
                    author = JsonSerializer.Deserialize<AuthorDto>(apiResponse);
                    _httpContextAccessor.HttpContext.Session.SetString("userSession", apiResponse);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message} {ex.InnerException} {ex.StackTrace}");
            }

        }
    }
}
