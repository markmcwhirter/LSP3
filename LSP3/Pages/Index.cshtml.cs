using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Web;

namespace LSP3.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IndexModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult OnGet()
    {
        bool IsAuthenticated = false;

        var sessionValue = _httpContextAccessor.HttpContext.Session.GetString("Authenticated");
        var currentUser = _httpContextAccessor.HttpContext.Session.GetString("currentuser");

        if (sessionValue != null) IsAuthenticated = bool.Parse(sessionValue);

        string url = "/Account/Login";

        if (IsAuthenticated)
        {
            if (currentUser == null)
                return Redirect(url);
            else
                return Page();
        }
        else
            return Redirect(url);
    }


}
