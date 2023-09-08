using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using static System.Reflection.Metadata.BlobBuilder;

namespace LSP3.Pages
{
    public class BookReportModel : MasterModel
    {
        [BindProperty]
        public List<BookSale> BookSaleData { get; set; }


        private readonly ILogger<IndexModel> _logger;
        public BookReportModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            BookSaleData = new List<BookSale>();

            HttpHelper helper = new HttpHelper();
            Extensions<List<BookSale>> bsExtention = new Extensions<List<BookSale>>();

            try
            {

                if (!base.IsAuthenticated)
                    return Redirect("/Account/Login");

                string apiResponse = await helper.Get($"http://localhost:5253/api/sale/getsales");

                if (!string.IsNullOrEmpty(apiResponse))
                    BookSaleData = bsExtention.Deserialize(apiResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
            }

            return Page();

        }
    }
}
