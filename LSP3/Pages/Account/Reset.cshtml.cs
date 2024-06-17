using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

using System.Text;
using System.Text.Encodings;
using System.Text.Unicode;

namespace LSP3.Pages.Account;

public class ResetModel(IOptions<AppSettings> appSettings, ILogger<ResetModel> logger, IHttpContextAccessor httpContextAccessor) : PageModel
{
    [BindProperty]
    public string? Username { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    private readonly ILogger<ResetModel> _logger = logger;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly AppSettings _appSettings = appSettings.Value;

    static readonly char[] padding = ['='];

    private readonly bool IsAdmin = false;

    public async Task<IActionResult> OnPost()
    {
        if (_httpContextAccessor.HttpContext != null)
        {

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

        HttpHelper helper = new();
        SessionHelper sessionHelper = new();

        if (username == null || password == null) return;

        try
        {
            byte[]? arrencrypted = await new EncryptionService().EncryptAsync(password);
            var encrypted = Convert.ToBase64String(arrencrypted).TrimEnd(padding).Replace('+', '-').Replace('/', '_');


            string apiResponse = await helper.Get(_appSettings.HostUrl + $"author/{username}/{encrypted}");


            if (apiResponse != null)
            {
                AuthorDto author = new Extensions<AuthorDto>().Deserialize(apiResponse);


                sessionHelper.SetSessionString(_httpContextAccessor, "Authenticated", "true");

                var isAdmin = author.Admin == "1" ? "true" : "false";
                sessionHelper.SetSessionString(_httpContextAccessor, "Admin", isAdmin);

            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

    }
}


