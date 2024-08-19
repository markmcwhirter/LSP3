using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System.Net.Http;
using System.Net;

namespace LSP3.Pages;


public class DeleteAuthor(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, ILogger<DeleteAuthor> logger, IHttpContextAccessor httpContextAccessor) : MasterModel(httpContextAccessor)
{
    private readonly ILogger<DeleteAuthor> _logger = logger;

    readonly HttpHelper helper = new();
    public AuthorDto? Results { get; set; }
    public string AddressString { get; set; }

    private readonly AppSettings _appSettings = appSettings.Value;

    [BindProperty]
    public List<BookDto>? Books { get; set; }

    [BindProperty]
    public int? BookCount { get; set; }

    public async Task OnGetAsync()
    {
        HttpHelper httphelper = new();
        SessionHelper sessionHelper = new();
        Extensions<AuthorDto> authorextensions = new();
        Extensions<List<BookDto>> bookextensions = new();


        if (!base.IsAuthenticated)
            return;

        if (!base.IsAdmin)
            return;


        string id = Request.Query["id"].ToString();
        var client = httpClientFactory.CreateClient("apiClient");

        var response = await httphelper.GetFactoryAsync(client, $"{_appSettings.HostUrl}author/{id}");

        if (response != null)
            Results = JsonConvert.DeserializeObject<AuthorDto>(response);

        AddressString = $"{Results.Address1},{Results.Address2} {Results.City}, {Results.State} {Results.ZIP} {Results.Country}";


        var apiResponse = await httphelper.GetFactoryAsync(client, $"{_appSettings.HostUrl}book/author/{id}");

        if (!string.IsNullOrEmpty(apiResponse))
        {

            Books = bookextensions.Deserialize(apiResponse);

            BookCount = Books.Count();
        }


    }
}