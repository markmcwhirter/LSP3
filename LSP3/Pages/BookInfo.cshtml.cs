using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace LSP3.Pages;


public class BookInfoModel : MasterModel
{

    readonly HttpHelper helper = new();
    public BookDto? Results { get; set; }
    private readonly AppSettings _appSettings;

    [BindProperty]
    public List<BookDto>? Books { get; set; }

    [BindProperty]
    public int? BookCount { get; set; }

    [BindProperty]
    public new AuthorDto? Author { get; set; }


    private readonly IHttpContextAccessor _httpContextAccessor;

    public BookInfoModel(IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task OnGetAsync()
    {
        Extensions<AuthorDto> authorextensions = new();
        HttpHelper helper = new();

        if (!base.IsAuthenticated) return;
        if (!base.IsAdmin) return;

        string id = Request.Query["bookid"].ToString();

        var response = await helper.Get(_appSettings.HostUrl + $"book/{id}");

        if (!string.IsNullOrEmpty(response))
            Results = JsonConvert.DeserializeObject<BookDto>(response);

        string apiResponse = await helper.Get(_appSettings.HostUrl + $"author/{Results.AuthorID}");

        if (!string.IsNullOrEmpty(apiResponse))
            Author = authorextensions.Deserialize(apiResponse);
    }
}
