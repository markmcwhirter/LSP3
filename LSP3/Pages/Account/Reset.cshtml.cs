using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

using System.Net;

namespace LSP3.Pages.Account;

public class ResetModel(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, ILogger<ResetModel> logger, IHttpContextAccessor httpContextAccessor) : PageModel
{
    [BindProperty]
    public string? Username { get; set; }

    [BindProperty]
    public string? Password { get; set; }
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly AppSettings _appSettings = appSettings.Value;

    static readonly char[] padding = ['='];

    public async Task<IActionResult> OnPost()
    {
        if (_httpContextAccessor.HttpContext != null)
        {

            await OnGetauthor(Username, Password);

            return RedirectToPage("/Index");
        }
        return Page();
    }

    public async Task OnGetauthor(string? username, string? password)
    {

        HttpHelper httphelper = new();
        SessionHelper sessionHelper = new();

        if (username == null || password == null) return;

        byte[]? arrencrypted = await new EncryptionService().EncryptAsync(password);
        var encrypted = Convert.ToBase64String(arrencrypted).TrimEnd(padding).Replace('+', '-').Replace('/', '_');

        var client = httpClientFactory.CreateClient("apiClient");

        var apiResponse = await httphelper.GetFactoryAsync(client, $"{_appSettings.HostUrl}author/{username}/{encrypted}");

        if (apiResponse != null)
        {
            AuthorDto author = new Extensions<AuthorDto>().Deserialize(apiResponse);


            sessionHelper.SetSessionString(_httpContextAccessor, "Authenticated", "true");

            var isAdmin = author.Admin == "1" ? "true" : "false";
            sessionHelper.SetSessionString(_httpContextAccessor, "Admin", isAdmin);

        }


    }
}


