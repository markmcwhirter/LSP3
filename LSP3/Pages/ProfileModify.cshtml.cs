using LSP3.Model;

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System.Net;

namespace LSP3.Pages;


public class ProfileModify(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, ILogger<Profile> logger, IHttpContextAccessor httpContextAccessor) : MasterModel(httpContextAccessor)
{
    private readonly ILogger<Profile> _logger = logger;

    readonly HttpHelper helper = new();
    public AuthorDto? Results { get; set; }
    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IHttpClientFactory _httpClientFactory;

    public async Task OnGetAsync()
    {
        HttpHelper httphelper = new();
        SessionHelper sessionHelper = new();

        string id = Request.Query["id"].ToString();


        if (!base.IsAuthenticated)
            return;

        var client = _httpClientFactory.CreateClient("apiClient");
        var apiResponse = await httphelper.GetFactoryAsync(client, $"{_appSettings.HostUrl}author/{id}");

        if (apiResponse != null)
            Results = JsonConvert.DeserializeObject<AuthorDto>(apiResponse);

    }
}