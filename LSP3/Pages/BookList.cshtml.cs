using LSP3.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LSP3.Pages
{
    public class BookListModel : MasterModel
    {
        [BindProperty]
        public List<BookDto> Books { get; set; }


        public BookListModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
        {
        }


        public async Task<IActionResult> OnGet()
        {

            try
            {

                if (!base.IsAuthenticated)
                    return Redirect("/Account/Login");


                string apiResponse = await new HttpHelper().Get($"https://localhost:7253/api/book/author/{Author.AuthorID}");
                if (!string.IsNullOrEmpty(apiResponse))
                {
                    Books = JsonSerializer.Deserialize<List<BookDto>>(apiResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });


                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return Page();


        }
    }
}
