using LSP3.Model;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace LSP3.Pages;

public class Profile : MasterModel
{
    private readonly ILogger<Profile> _logger;

    readonly HttpHelper helper = new();
    public AuthorDto? Results { get; set; }
    private readonly AppSettings _appSettings;
    public Profile(IOptions<AppSettings> appSettings, ILogger<Profile> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public async Task OnGetAsync()
    {

        try
        {
            string id = Request.Query["id"].ToString();
            var response = await helper.Get(_appSettings.HostUrl + $"author/{id}");

            if (response != null)
                Results = JsonConvert.DeserializeObject<AuthorDto>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }


    }
}
