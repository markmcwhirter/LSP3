using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            HttpHelper helper = new HttpHelper();

            if (_httpContextAccessor.HttpContext != null)
            {
                if( string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) )
                {
                    Username = "mark";
                    Password = "Joellevivi1";
                }

                var author = OnGetauthor(Username, Password);

                helper.SetSessionString(_httpContextAccessor, "Authenticated", "true");
                await OnGetauthor(Username, Password);

                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task OnGetauthor(string? username, string? password)
        {
            AuthorDto author = new AuthorDto();

            HttpHelper helper = new HttpHelper();

            if (username == null || password == null) return;

            try
            {
                byte[]? arrencrypted = await new EncryptionService().EncryptAsync(password);
                var encrypted = Convert.ToBase64String(arrencrypted).TrimEnd(padding).Replace('+', '-').Replace('/', '_');


                string apiResponse = await helper.Get($"http://localhost:5253/api/author/{username}/{encrypted}");


                if (apiResponse != null)
                {
                    author = new Extensions<AuthorDto>().Deserialize(apiResponse);
                    await helper.Get($"http://localhost:5253/api/author/{username}/{encrypted}");
                    helper.SetSessionString(_httpContextAccessor, "userSession", apiResponse);
                    helper.SetSessionString(_httpContextAccessor, "Authenticated", "true");

                    if ( author.Admin != null)
                        helper.SetSessionString(_httpContextAccessor, "Admin", "true");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message} {ex.InnerException} {ex.StackTrace}");
            }

        }
    }
}
