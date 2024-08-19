using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

using System.Net.Http;

using System.Net;

namespace LSP3.Pages;

public class DisplayTextModel(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, ILogger<DisplayTextModel> logger, IHttpContextAccessor httpContextAccessor) : PageModel
{
    [BindProperty]
    public AuthorDto? Author { get; set; }

    [BindProperty]
    public BookDto? Book { get; set; }


    [BindProperty]
    public string? Text { get; set; }

    private readonly ILogger<DisplayTextModel> _logger = logger;

    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<IActionResult> OnGet()
    {
        string field = System.Web.HttpUtility.HtmlDecode(Request.Query["field"].ToString());
        string bookid = System.Web.HttpUtility.HtmlDecode(Request.Query["bookid"].ToString());
        string authorid = System.Web.HttpUtility.HtmlDecode(Request.Query["authorid"].ToString());
        string table = System.Web.HttpUtility.HtmlDecode(Request.Query["table"].ToString());

        string apiResponse;

        HttpHelper helper = new();
        SessionHelper sessionHelper = new();
        Extensions<AuthorDto> authorextensions = new();
        Extensions<BookDto> bookextensions = new();


        if (!sessionHelper.IsAuthenticated(_httpContextAccessor))
            return Redirect("/Account/Login");

        var client = httpClientFactory.CreateClient("apiClient");


        if (table.Equals("author", StringComparison.CurrentCultureIgnoreCase))
        {
            apiResponse = await helper.GetFactoryAsync(client, $"{_appSettings.HostUrl}author/{authorid}");

            if (!string.IsNullOrEmpty(apiResponse))
                Author = authorextensions.Deserialize(apiResponse);
        }
        else
        {
            apiResponse = await helper.GetFactoryAsync(client, $"{_appSettings.HostUrl}book/{bookid}");

            if (!string.IsNullOrEmpty(apiResponse))
                Book = bookextensions.Deserialize(apiResponse);

        }
        field = field.ToLower();

        switch (field)
        {
            case "coveridea":
                Text = Book.CoverIdea ?? "No CoverIdea available";
                break;
            case "authorbio":
                Text = Book.AuthorBio ?? "No Biography available";
                break;
            case "description":
                Text = Book.Description ?? "No Description available";
                break;
            case "notes":
                Text = Book.Notes ?? "No Notes available";
                break;

        }


        return Page();


    }
}
