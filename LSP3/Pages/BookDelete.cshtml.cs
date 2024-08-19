using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System.Net.Http;
using System.Net;

namespace LSP3.Pages;


public class BookDeleteModel(IHttpClientFactory httpClientFactory,IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor) : MasterModel(httpContextAccessor)
{

    
    public BookDto? Results { get; set; }
    private readonly AppSettings _appSettings = appSettings.Value;

    [BindProperty]
    public List<BookDto>? Books { get; set; }

    [BindProperty]
    public int? BookCount { get; set; }

    public async Task OnGetAsync()
    {

        HttpHelper httphelper = new();

        if (!base.IsAuthenticated) return;
        if (!base.IsAdmin) return;

        string id = Request.Query["bookid"].ToString();

        var client = httpClientFactory.CreateClient("apiClient");

        var response = await httphelper.GetFactoryAsync(client, $"{_appSettings.HostUrl}book/{id}");

        if (!string.IsNullOrEmpty(response))
            Results = JsonConvert.DeserializeObject<BookDto>(response);


    }
}
