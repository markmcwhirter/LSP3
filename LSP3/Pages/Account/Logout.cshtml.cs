using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LSP3.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly ILogger<LogoutModel> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    SessionHelper sessionHelper = new SessionHelper();

    public LogoutModel(ILogger<LogoutModel> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult OnGet() 
    {
        SessionHelper helper = new();
        var username = helper.GetSessionString(_httpContextAccessor, "Username");
        _logger.LogInformation($"*** Logging out: Username: {username}");

        if (_httpContextAccessor.HttpContext != null)
        {

            sessionHelper.SetSessionString(_httpContextAccessor, "Authenticated", "false");
            sessionHelper.SetSessionString(_httpContextAccessor, "Admin", "false");
        }
        return Redirect("/Account/Login");
    }

}
