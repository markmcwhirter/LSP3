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

    public AuthorDto Author = new AuthorDto();
    public string CurrentUser = "";

    public MasterModel(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public override void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
        //...
    }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        HttpHelper helper = new HttpHelper();

        if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Session != null)
        {


            authenticated = helper.GetSessionString(_httpContextAccessor,"Authenticated");

            if (authenticated != null)
                IsAuthenticated = authenticated == null ? false : bool.Parse(authenticated);


            var tmpUserSession = helper.GetSessionString(_httpContextAccessor, "userSession");


            if (tmpUserSession != null)
            {
                Author = new Extensions<AuthorDto>().Deserialize(tmpUserSession);
                CurrentUser = Author.Username;
            }
        }

    }
}

