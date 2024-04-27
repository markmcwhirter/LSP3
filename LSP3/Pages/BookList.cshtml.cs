using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class BookListModel : MasterModel
{
    [BindProperty]
    public List<BookDto>? Books { get; set; }


    private readonly ILogger<BookListModel> _logger;

    private readonly AppSettings _appSettings;

    public BookListModel(IOptions<AppSettings> appSettings, ILogger<BookListModel> logger, IHttpContextAccessor httpContextAccessor) : base( httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }


    public async Task<IActionResult> OnGet()
    {
        HttpHelper helper = new();
        Extensions<List<BookDto>> extensions = new();

        try
        {

            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            string apiResponse = await helper.Get(_appSettings.HostUrl + $"author/{Author.AuthorID}");
            Books = extensions.Deserialize(apiResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }

        return Page();


    }
}
