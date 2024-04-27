using LSP3.Model;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace LSP3.Pages;


public class ProfileModify : MasterModel
{
    private readonly ILogger<Profile> _logger;

    HttpHelper helper = new HttpHelper();
    public AuthorDto? Results { get; set; }
    private readonly AppSettings _appSettings;
    public ProfileModify(IOptions<AppSettings> appSettings, ILogger<Profile> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        string id = Request.Query["id"].ToString();

        try
        {

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