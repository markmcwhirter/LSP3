using LSP3.Model;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LSP3.Pages;


public class MasterModel : PageModel
{

    public readonly IHttpContextAccessor _httpContextAccessor;

    public bool IsAuthenticated = false;
    public bool IsAdmin = false;
    public int AuthorId = 0;

    public AuthorDto Author = new();
    public string CurrentUser = "";

    public MasterModel(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }



    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {

        SessionHelper sessionHelper = new();

        if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Session != null)
        {

            IsAuthenticated = sessionHelper.IsAuthenticated(_httpContextAccessor);
            IsAdmin = sessionHelper.IsAdmin(_httpContextAccessor);
            AuthorId = sessionHelper.GetAuthorId(_httpContextAccessor);
        }

    }
}

