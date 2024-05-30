using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LSP3.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    SessionHelper sessionHelper = new SessionHelper();

    public LogoutModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult OnGet() {

        if (_httpContextAccessor.HttpContext != null)
        {
            sessionHelper.SetSessionString(_httpContextAccessor, "Authenticated", "false");
            sessionHelper.SetSessionString(_httpContextAccessor, "Admin", "false");
            sessionHelper.SetSessionString(_httpContextAccessor, "AuthorId", "0");
        }
        return Redirect("/Account/Login");
    }

}
