using LSP3.Model;

using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class SearchModel : MasterModel
{
    private readonly ILogger<SearchModel> _logger;

    readonly HttpHelper helper = new();
    public AuthorDto? Results { get; set; }
    private readonly AppSettings _appSettings;
    public SearchModel(IOptions<AppSettings> appSettings, ILogger<SearchModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
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