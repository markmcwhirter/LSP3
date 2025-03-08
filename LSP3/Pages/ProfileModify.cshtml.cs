using LSP3.Model;

using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace LSP3.Pages;


public class ProfileModify(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, ILogger<Profile> logger, IHttpContextAccessor httpContextAccessor) : MasterModel(httpContextAccessor)
{
    public AuthorDto? Results { get; set; }
    private readonly AppSettings _appSettings = appSettings.Value;
   

    public async Task OnGetAsync()
    {
        HttpHelper httphelper = new();

        string id = Request.Query["id"].ToString();


        if (!base.IsAuthenticated)
            return;

        var client = httpClientFactory.CreateClient("apiClient");
        var apiResponse = await httphelper.GetFactoryAsync(client, $"{_appSettings.HostUrl}author/{id}");

        if (apiResponse != null)
            Results = JsonConvert.DeserializeObject<AuthorDto>(apiResponse);

    }
}