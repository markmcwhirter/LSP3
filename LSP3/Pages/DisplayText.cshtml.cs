using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using static System.Reflection.Metadata.BlobBuilder;

namespace LSP3.Pages;

public class DisplayTextModel : MasterModel
{
    [BindProperty]
    public AuthorDto? Author { get; set; }

    [BindProperty]
    public BookDto? Book { get; set; }


    [BindProperty]
    public string? Text { get; set; }

    private readonly ILogger<DisplayTextModel> _logger;

    private readonly AppSettings _appSettings;

    public DisplayTextModel(IOptions<AppSettings> appSettings, ILogger<DisplayTextModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        string field = System.Web.HttpUtility.HtmlDecode(Request.Query["field"].ToString());
        string bookid = System.Web.HttpUtility.HtmlDecode(Request.Query["bookid"].ToString());
        string authorid = System.Web.HttpUtility.HtmlDecode(Request.Query["authorid"].ToString());
        string table = System.Web.HttpUtility.HtmlDecode(Request.Query["table"].ToString());

        string apiResponse = "";

        HttpHelper helper = new();
        Extensions<AuthorDto> authorextensions = new();
        Extensions<BookDto> bookextensions = new();

        try
        {

            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            if (table.ToLower() == "author")
            {
                apiResponse = await helper.Get(_appSettings.HostUrl + $"author/{authorid}");

                if (!string.IsNullOrEmpty(apiResponse))
                    Author = authorextensions.Deserialize(apiResponse);
            }
            else
            {
                apiResponse = await helper.Get(_appSettings.HostUrl + $"book/{bookid}");

                if (!string.IsNullOrEmpty(apiResponse))
                    Book = bookextensions.Deserialize(apiResponse);

            }
            field = field.ToLower();

            switch(field)
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
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }

        return Page();


    }
}
