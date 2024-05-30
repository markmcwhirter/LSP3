using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class BookSalesModel : MasterModel
{
    [BindProperty]
    public List<BookSale>? BookSaleData { get; set; }
    public bool IsReadOnly { get; set; }

    [BindProperty]
    public BookDto? Book { get; set; }

    [BindProperty]
    public int? AuthorId { get; set; }


    private readonly ILogger<BookSalesModel> _logger;

    private readonly AppSettings _appSettings;
    private SessionHelper sessionHelper = new();
    private readonly IHttpContextAccessor _httpContextAccessor;


    public BookSalesModel(IOptions<AppSettings> appSettings, ILogger<BookSalesModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;

    }

    public async Task<IActionResult> OnGet()
    {
        BookSaleData = new List<BookSale>();

        HttpHelper helper = new();
        Extensions<List<BookSale>> bsExtention = new Extensions<List<BookSale>>();

        try
        {

            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            if (!base.IsAdmin)
                return Redirect("/Index");



            string apiResponse = await helper.Get(_appSettings.HostUrl + $"sale/getsales");

            if (!string.IsNullOrEmpty(apiResponse))
            {
                var temp = bsExtention.Deserialize(apiResponse);
                BookSaleData = temp
                    .OrderBy(x => x.Author)
                    .ThenBy(y => y.Title)
                    .ThenBy(z => z.SaleID)
                    .ToList();

                string oldauthor = "";

                foreach (var item in BookSaleData)
                {
                    if (item.Author.Trim() == ",")
                        item.Author = "(None)";

                    if (item.Author.Trim() != oldauthor)
                        oldauthor = item.Author;
                    else
                        item.Author = "";

                    item.SalesDate = item.SalesDate.Replace(" 00:00:00", "");
                }
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }

        return Page();

    }
}
