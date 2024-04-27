using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class DeleteAuthor : MasterModel
{


    public IList<AuthorListResultsModel>? Results { get; set; }

    HttpHelper helper = new HttpHelper();
    Extensions<List<AuthorDto>> extensions = new Extensions<List<AuthorDto>>();
  

    public DeleteAuthor( IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {

    }


    public IActionResult OnGet()
    {
        return RedirectToPage("/AuthorSearch");
    }
}
