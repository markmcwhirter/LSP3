using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class AdminModel : MasterModel
{
    [BindProperty]
    public new AuthorDto? Author { get; set; }

    [BindProperty]
    public List<BookSummaryModel>? Books { get; set; }

    [BindProperty]
    public SalesSummaryModel Sales { get; set; }


    private readonly ILogger<AdminModel> _logger;

    private readonly AppSettings _appSettings;

    public AdminModel(IOptions<AppSettings> appSettings, ILogger<AdminModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        HttpHelper helper = new();
        Extensions<AuthorDto> authorextensions = new();
        Extensions<List<BookSummaryModel>> bookextensions = new();


        try
        {
            Author = new AuthorDto();
            Books = new List<BookSummaryModel>();
            Sales = new SalesSummaryModel();

            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            string apiResponse = await helper.Get(_appSettings.HostUrl + $"author/{base.Author.AuthorID}");

            if (!string.IsNullOrEmpty(apiResponse))
                Author = authorextensions.Deserialize(apiResponse);

            apiResponse = await helper.Get(_appSettings.HostUrl + $"book/author/{base.Author.AuthorID}");

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
