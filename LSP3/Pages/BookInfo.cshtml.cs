using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System.Net.Http;
using System.Net;

namespace LSP3.Pages;


public class BookInfoModel(IHttpClientFactory httpClientFactory,IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor) : MasterModel(httpContextAccessor)
{

    readonly HttpHelper helper = new();
    public BookDto? Results { get; set; }
    private readonly AppSettings _appSettings = appSettings.Value;

    [BindProperty]
    public List<BookDto>? Books { get; set; }

    [BindProperty]
    public int? BookCount { get; set; }

    [BindProperty]
    public new AuthorDto? Author { get; set; }

    public async Task OnGetAsync()
    {
        Extensions<AuthorDto> authorextensions = new();
        HttpHelper httphelper = new();

        if (!base.IsAuthenticated) return;
        if (!base.IsAdmin) return;

        string id = Request.Query["bookid"].ToString();


        var client = httpClientFactory.CreateClient("apiClient");

        string response = await httphelper.GetFactoryAsync(client, $"{_appSettings.HostUrl}book/{id}");

        if (!string.IsNullOrEmpty(response))
            Results = JsonConvert.DeserializeObject<BookDto>(response);

        response = await httphelper.GetFactoryAsync(client, $"{_appSettings.HostUrl}author/{Results.AuthorID}");

        if (!string.IsNullOrEmpty(response))
            Author = authorextensions.Deserialize(response);
    }
}
