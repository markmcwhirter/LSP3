using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace LSP3.Pages;

public class HelpModel : MasterModel
{
    [BindProperty]
    public string? From { get; set; }

    [BindProperty]
    public string? FromEmail{ get; set; }
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HelpModel(IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public IActionResult OnGet()
    {
        HttpHelper helper = new();
        SessionHelper   sessionHelper = new();

        if (!sessionHelper.IsAuthenticated(_httpContextAccessor))
            return Redirect("/Account/Login");

        From = base.CurrentUser;
        FromEmail = base.Author.Email;

        return Page();
    }
}

public class ContactFormModel
{
    public string? Name { get; set; }
    public string? Subject { get; set; }
    public string? Email { get; set; }
    public string? Body { get; set; }
}
