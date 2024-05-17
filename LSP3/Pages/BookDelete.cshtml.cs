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



    public BookDeleteModel(IOptions<AppSettings> appSettings, ILogger<BookDeleteModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }
    public async Task OnGetAsync()
    {


        try
        {
            HttpHelper helper = new();
            Extensions<AuthorDto> authorextensions = new();
            Extensions<List<BookDto>> bookextensions = new();



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
