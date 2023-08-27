using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Web;
using LSP3.Model;
using System.Text.Json;
using static System.Reflection.Metadata.BlobBuilder;

namespace LSP3.Pages;


public class MasterModel : PageModel
{
    public readonly ILogger<IndexModel> _logger;
    public readonly IHttpContextAccessor _httpContextAccessor;

    public string authenticated = "";
    public string currentUser = "";
    public bool IsAuthenticated = false;

    public AuthorDto Author = new AuthorDto();
    public string CurrentUser = "";

    public MasterModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
    public override void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
        //...
    }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {

        if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Session != null)
        {

            authenticated = _httpContextAccessor.HttpContext.Session.GetString("Authenticated");
            if (authenticated != null)
                IsAuthenticated = authenticated == null ? false : bool.Parse(authenticated);


            var tmpUserSession = _httpContextAccessor.HttpContext.Session.GetString("userSession");
            if (tmpUserSession != null)
                Author = JsonSerializer.Deserialize<AuthorDto>(tmpUserSession,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });


            var tmpCurrentUser = _httpContextAccessor.HttpContext.Session.GetString("currentuser");

            if (tmpCurrentUser != null)
                CurrentUser = tmpCurrentUser;
        }

    }
}

