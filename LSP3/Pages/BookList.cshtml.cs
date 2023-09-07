using LSP3.Model;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Text.Json;

namespace LSP3.Pages
{
    public class BookListModel : MasterModel
    {
        [BindProperty]
        public List<BookDto> Books { get; set; }


        private readonly ILogger<BookListModel> _logger;
        public BookListModel(ILogger<BookListModel> logger, IHttpContextAccessor httpContextAccessor) : base( httpContextAccessor)
        {
            _logger = logger;
        }


        public async Task<IActionResult> OnGet()
        {
            HttpHelper helper = new HttpHelper();
            Extensions<List<BookDto>> extensions = new Extensions<List<BookDto>>();

            try
            {

                if (!base.IsAuthenticated)
                    return Redirect("/Account/Login");

                string apiResponse = await helper.Get($"http://localhost:5253/api/book/author/{Author.AuthorID}");
                Books = extensions.Deserialize(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
            }

            return Page();


        }
    }
}
