using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

using System.Net;

namespace LSP3.Pages;

public class AdminModel(IHttpClientFactory httpClientFactory,IOptions<AppSettings> appSettings, ILogger<AdminModel> logger, IHttpContextAccessor httpContextAccessor) : MasterModel(httpContextAccessor)
{
    [BindProperty]
    public new AuthorDto? Author { get; set; }

    [BindProperty]
    public List<BookSummaryModel>? Books { get; set; }

    [BindProperty]
    public SalesSummaryModel Sales { get; set; }


    private readonly ILogger<AdminModel> _logger = logger;

    private readonly AppSettings _appSettings = appSettings.Value;

    public async Task<IActionResult> OnGet()
    {

        HttpHelper httphelper = new();
        SessionHelper sessionHelper = new();
        Extensions<AuthorDto> authorextensions = new();
        Extensions<List<BookSummaryModel>> bookextensions = new();

        string? authorid = "";

        Author = new AuthorDto();
        Books = new List<BookSummaryModel>();
        Sales = new SalesSummaryModel();


        if (!base.IsAuthenticated)
            return Redirect("/Account/Login");

        if (!base.IsAdmin)
            return Redirect("/Index");


        if (Request.Query.ContainsKey("authorid"))
            authorid = Request.Query["authorid"].ToString();
        else
            authorid = base.AuthorId.ToString();

        var client = httpClientFactory.CreateClient("apiClient");

        var apiResponse = await httphelper.GetFactoryAsync(client, $"{_appSettings.HostUrl}author/{authorid}");

        if (!string.IsNullOrEmpty(apiResponse))
            Author = authorextensions.Deserialize(apiResponse);

        apiResponse = await httphelper.GetFactoryAsync(client, $"{_appSettings.HostUrl}book/author/{authorid}");

        if (!string.IsNullOrEmpty(apiResponse))
            Books = bookextensions.Deserialize(apiResponse);

        foreach (var b in Books)
        {
            var cover = b.Cover;
        }


        return Page();
    }
}
