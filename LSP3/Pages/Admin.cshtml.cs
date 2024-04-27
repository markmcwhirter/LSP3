using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class AdminModel : MasterModel
{
    private readonly ILogger<AdminModel> _logger;
    private readonly AppSettings _appSettings;

    public AdminModel(IOptions<AppSettings> appSettings, ILogger<AdminModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        HttpHelper helper = new();
        Extensions<AuthorDto> authorextensions = new();
 
        try
        {

            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            string apiResponse = await helper.Get(_appSettings.HostUrl + $"author/{base.Author.AuthorID}");

            if (!string.IsNullOrEmpty(apiResponse))
            {

                Author = authorextensions.Deserialize(apiResponse);
            }


        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }

        return Page();
    }
}
