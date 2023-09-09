using LSP3.Model;

using Microsoft.AspNetCore.Mvc;

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

        //public async Task<IActionResult> OnGet()
        //{
        //    BookSaleData = new List<BookSale>();

        //    HttpHelper helper = new HttpHelper();
        //    Extensions<List<BookSale>> bsExtention = new Extensions<List<BookSale>>();

        //    try
        //    {

        //        if (!base.IsAuthenticated)
        //            return Redirect("/Account/Login");

        //        string apiResponse = await helper.Get($"http://localhost:5253/api/sale/getsales");

        //        if (!string.IsNullOrEmpty(apiResponse))
        //        {
        //            var temp = bsExtention.Deserialize(apiResponse);
        //            BookSaleData = temp
        //                .OrderBy(x => x.Author)
        //                .ThenBy(y => y.Title)
        //                .ThenBy(z => z.SaleID)
        //                .ToList();

        //            string oldauthor = "";

        //            foreach( var item in BookSaleData)
        //            {
        //                if (item.Author.Trim() == ",")
        //                    item.Author = "(None)";

        //                if (item.Author.Trim() != oldauthor)
        //                    oldauthor = item.Author;
        //                else
        //                    item.Author = "";

        //                item.SalesDate = item.SalesDate.Replace(" 00:00:00", "");
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        //    }

        //    return Page();

        //}
    }
}
