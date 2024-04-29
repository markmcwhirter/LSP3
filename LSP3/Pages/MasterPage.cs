using LSP3.Model;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LSP3.Pages;


public class MasterModel : PageModel
{

    public readonly IHttpContextAccessor _httpContextAccessor;

    public string authenticated = "";
    public string currentUser = "";
    public bool IsAuthenticated = false;
    public bool IsAdmin = false;

    public AuthorDto Author = new();
    public string CurrentUser = "";

    public MasterModel(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }



    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        HttpHelper helper = new();

        if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Session != null)
        {
           
            string tmpUserSession = helper.GetCookie(_httpContextAccessor, "userSession");

            if (!string.IsNullOrEmpty(tmpUserSession))
            {
                Author = new Extensions<AuthorDto>().Deserialize(tmpUserSession);
                CurrentUser = Author.Username ?? "";
                IsAuthenticated = true;
            }
        }

    }
}

