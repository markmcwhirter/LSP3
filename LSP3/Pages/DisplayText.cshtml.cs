using LSP3.Model;

using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class DisplayTextModel : MasterModel
{
    private readonly AppSettings _appSettings;

    public DisplayTextModel(IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
    }

    public void OnGet()
    {
        string imageName = System.Web.HttpUtility.HtmlDecode(Request.Query["target"].ToString());
        var imageType = Path.GetExtension(imageName).ToLower();
    }
}
