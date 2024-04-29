using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class DeleteAuthor : MasterModel
{


    public IList<AuthorListResultsModel>? Results { get; set; }

    readonly HttpHelper helper = new();
    readonly Extensions<List<AuthorDto>> extensions = new();
  

    public DeleteAuthor( IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {

    }


    public IActionResult OnGet()
    {
        return RedirectToPage("/AuthorSearch");
    }
}
