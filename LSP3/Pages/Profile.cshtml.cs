using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

using System.Net.Http;

using System.Net;

namespace LSP3.Pages;

public class Profile(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, ILogger<Profile> logger, IHttpContextAccessor httpContextAccessor) : MasterModel(httpContextAccessor)
{
    public AuthorDto? Results { get; set; }

    public async Task OnGetAsync()
    {

        string id = "";
        HttpHelper httphelper = new();

        if (!base.IsAuthenticated)
            return;

        Results = new AuthorDto();

        if (!string.IsNullOrEmpty(Request.Query["id"]))
        {
            id = Request.Query["id"].ToString();

            var client = httpClientFactory.CreateClient("apiClient");
            var apiResponse = await httphelper.GetFactoryAsync(client, $"{appSettings.Value.HostUrl}author/{id}");

            if (apiResponse != null)
                Results = JsonConvert.DeserializeObject<AuthorDto>(apiResponse);
        }


    }


    public IActionResult OnPost()
    {
        return Redirect("/Index");

    }
}
