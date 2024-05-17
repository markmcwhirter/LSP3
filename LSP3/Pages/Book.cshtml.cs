using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using static System.Reflection.Metadata.BlobBuilder;

namespace LSP3.Pages;

public class BookModel : MasterModel
{
    public bool IsReadOnly { get; set; }

    [BindProperty]
    public BookDto? Book { get; set; }

    [BindProperty]
    public int? AuthorId { get; set; }


    private readonly ILogger<BookModel> _logger;

    private readonly AppSettings _appSettings;

    public BookModel(IOptions<AppSettings> appSettings, ILogger<BookModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet(int? bookid, int? authorid)
    {


        try
        {
            HttpHelper helper = new();
            Extensions<BookDto> bookextensions = new Extensions<BookDto>();


            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            IsReadOnly = false;
            Book = new BookDto();

            if (Request.Query.ContainsKey("authorid"))
            {
                AuthorId = authorid;
                Book.AuthorID = authorid ?? 0;
            }

            if (Request.Query.ContainsKey("bookid"))
            {
                IsReadOnly = true;

                var apiResponse = await helper.Get(_appSettings.HostUrl + $"book/{bookid}");

                if (!string.IsNullOrEmpty(apiResponse))
                    Book = bookextensions.Deserialize(apiResponse);
            }


        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }

        return Page();
    }
}
