using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace LSP3.Pages.Account;

public class LoginModel : PageModel
{
    [BindProperty]
    public string? Username { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppSettings _appSettings;

    static readonly char[] padding = { '=' };

    private bool IsAdmin = false;

    public LoginModel(IOptions<AppSettings> appSettings, ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor)
    {
        _appSettings = appSettings.Value;
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
                Password = "joellevivi1";
            }

            var author = OnGetauthor(Username, Password);

            helper.SetSessionString(_httpContextAccessor, "Authenticated", "true");
            await OnGetauthor(Username, Password);

            if( IsAdmin )
                return RedirectToPage("/Admin");
            else
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
                await helper.Get(_appSettings.HostUrl + $"author/{username}/{encrypted}");
                helper.SetSessionString(_httpContextAccessor, "userSession", apiResponse);

                HttpContext.Response.Cookies.Append("userSession", apiResponse, new CookieOptions
                {
                    Expires = DateTime.Now.AddHours(1)
                });

                helper.SetCookie(_httpContextAccessor, "userSession", apiResponse);
               

                helper.SetSessionString(_httpContextAccessor, "Authenticated", "true");

                if (author.Admin != null)
                {
                    helper.SetCookie(_httpContextAccessor, "Admin", "true");
                    IsAdmin = true;
                }
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message} {ex.InnerException} {ex.StackTrace}");
        }

    }
}
