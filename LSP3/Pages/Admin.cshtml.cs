using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using static System.Reflection.Metadata.BlobBuilder;

namespace LSP3.Pages
{
    public class AdminModel : MasterModel
    {
        private readonly ILogger<AdminModel> _logger;
        public AdminModel(ILogger<AdminModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            HttpHelper helper = new HttpHelper();
            Extensions<AuthorDto> authorextensions = new Extensions<AuthorDto>();
            Extensions<List<BookDto>> bookextensions = new Extensions<List<BookDto>>();

            try
            {

                if (!base.IsAuthenticated)
                    return Redirect("/Account/Login");

                string apiResponse = await helper.Get($"http://localhost:5253/api/author/{base.Author.AuthorID}");

                if (!string.IsNullOrEmpty(apiResponse))
                {

                    Author = authorextensions.Deserialize(apiResponse);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
            }

            return Page();
        }
    }
}
