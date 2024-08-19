using LSP3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

using System.Net.Http;

using System.Net;

namespace LSP3.Pages;

public class IndexModel(IHttpClientFactory httpClientFactory,IOptions<AppSettings> appSettings, ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor) : MasterModel( httpContextAccessor)
{
    [BindProperty]
    public bool IsAdmin { get; set; }

    [BindProperty]
    public new AuthorDto? Author { get; set; }

    [BindProperty]
    public List<BookSummaryModel>? Books { get; set; }

    [BindProperty]
    public SalesSummaryModel Sales { get; set; }


    private readonly ILogger<IndexModel> _logger = logger;

    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<IActionResult> OnGet()
    {
        try
        {
            HttpHelper helper = new();
            SessionHelper sessionHelper = new();
            Extensions<AuthorDto> authorextensions = new();
            Extensions<List<BookSummaryModel>> bookextensions = new();

            Author = new AuthorDto();
            Books = new List<BookSummaryModel>();
            Sales = new SalesSummaryModel();


            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

             int authorId = int.Parse(sessionHelper.GetSessionString(_httpContextAccessor, "AuthorId"));

            var client = httpClientFactory.CreateClient("apiClient");
            var apiResponse = await helper.GetFactoryAsync(client, $"{_appSettings.HostUrl}author/{authorId}");

            if (!string.IsNullOrEmpty(apiResponse))
                Author = authorextensions.Deserialize(apiResponse);

            apiResponse = await helper.GetFactoryAsync(client, $"{_appSettings.HostUrl}book/author/{authorId}");

            if (!string.IsNullOrEmpty(apiResponse))
                Books = bookextensions.Deserialize(apiResponse);

            
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }

        return Page();

    }
}
