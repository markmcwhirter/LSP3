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

    public void OnGet() { }

    public async Task<IActionResult> OnPost()
    {
        AuthorDto author = new AuthorDto();

        HttpHelper helper = new HttpHelper();


        if (_httpContextAccessor.HttpContext != null)
        {
            helper.SetCookie(_httpContextAccessor, "userSession", "");
        }

        return Redirect("/Account/Login");
    }
}
