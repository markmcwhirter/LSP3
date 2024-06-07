using LSP3.Model;
using LSP3.Pages;
using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class SalesChart : MasterModel
{
    private readonly ILogger<SalesChart> _logger;

    readonly HttpHelper helper = new();

    private readonly AppSettings _appSettings;
    public SalesChart(IOptions<AppSettings> appSettings, ILogger<SalesChart> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
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