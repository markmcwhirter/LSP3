using LSP3.Model;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;
    private readonly AppSettings _appSettings;
    public PrivacyModel(IOptions<AppSettings> appSettings, ILogger<PrivacyModel> logger)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public void OnGet()
    {
    }
}

