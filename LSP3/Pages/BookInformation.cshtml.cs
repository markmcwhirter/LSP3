using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class BookInformationModel : MasterModel
{

    public BookInformationModel(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {

    }

    //public async Task<IActionResult> OnGet()
    //{
    //    HttpHelper helper = new HttpHelper();
    //    Extensions<AuthorDto> authorextensions = new Extensions<AuthorDto>();
    //    Extensions<List<BookDto>> bookextensions = new Extensions<List<BookDto>>();

    //    try
    //    {

    //        if (!base.IsAuthenticated)
    //            return Redirect("/Account/Login");

    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
    //    }

    //    return Page();
    //}
}
