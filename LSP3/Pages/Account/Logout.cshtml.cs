using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LSP3.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogoutModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult OnGet() {
        HttpHelper helper = new();


        if (_httpContextAccessor.HttpContext != null)
        {
            helper.SetCookie(_httpContextAccessor, "userSession", "");
            helper.SetCookie(_httpContextAccessor, "Authenticated", "");
            helper.SetCookie(_httpContextAccessor, "Admin", "");
        }

        return Redirect("/Account/Login");
    }

    public IActionResult OnPost()
    {
        HttpHelper helper = new();


        if (_httpContextAccessor.HttpContext != null)
        {
            helper.SetCookie(_httpContextAccessor, "userSession", "");
            helper.SetCookie(_httpContextAccessor, "Authenticated", "");
            helper.SetCookie(_httpContextAccessor, "Admin", "");
        }

        return Redirect("/Account/Login");
    }
}
