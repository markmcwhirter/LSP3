using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace LSP3.Pages;


public class BookDeleteModel : MasterModel
{
    private readonly ILogger<BookDeleteModel> _logger;
    readonly HttpHelper helper = new();
    public BookDto? Results { get; set; }
    private readonly AppSettings _appSettings;

    [BindProperty]
    public List<BookDto>? Books { get; set; }

    [BindProperty]
    public int? BookCount { get; set; }

    private readonly IHttpContextAccessor _httpContextAccessor;

    public BookDeleteModel(IOptions<AppSettings> appSettings, ILogger<BookDeleteModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task OnGetAsync()
    {


        try
        {
            HttpHelper helper = new();
            SessionHelper sessionHelper = new();
            Extensions<AuthorDto> authorextensions = new();
            Extensions<List<BookDto>> bookextensions = new();

            if (!base.IsAuthenticated) return;
            if (!base.IsAdmin) return;

            string id = Request.Query["bookid"].ToString();

            var response = await helper.Get(_appSettings.HostUrl + $"book/{id}");

            if (!string.IsNullOrEmpty(response))
                Results = JsonConvert.DeserializeObject<BookDto>(response);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }

    }
}
