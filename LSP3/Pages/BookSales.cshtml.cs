using LSP3.Model;

using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class SalesModel : MasterModel
{
    private readonly ILogger<SalesModel> _logger;

    readonly HttpHelper helper = new();

    private readonly AppSettings _appSettings;
    public SalesModel(IOptions<AppSettings> appSettings, ILogger<SalesModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }
    public void OnGet()
    {

        if (!base.IsAuthenticated)
            return;

        if (!base.IsAdmin)
            return;
    }
}