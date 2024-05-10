using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using static System.Reflection.Metadata.BlobBuilder;

namespace LSP3.Pages;


public class DeleteAuthor : MasterModel
{
    private readonly ILogger<DeleteAuthor> _logger;

    readonly HttpHelper helper = new();
    public AuthorDto? Results { get; set; }
    public string AddressString { get; set; }

    private readonly AppSettings _appSettings;

    [BindProperty]
    public List<BookDto>? Books { get; set; }

    [BindProperty]
    public int? BookCount { get; set; }



    public DeleteAuthor(IOptions<AppSettings> appSettings, ILogger<DeleteAuthor> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        HttpHelper helper = new();
        Extensions<AuthorDto> authorextensions = new();
        Extensions<List<BookDto>> bookextensions = new();



        string id = Request.Query["id"].ToString();

        try
        {

            var response = await helper.Get(_appSettings.HostUrl + $"author/{id}");

            if (response != null)
                Results = JsonConvert.DeserializeObject<AuthorDto>(response);

            AddressString = $"{Results.Address1},{Results.Address2} {Results.City}, {Results.State} {Results.ZIP} {Results.Country}";

            var apiResponse = await helper.Get(_appSettings.HostUrl + $"book/author/{id}");

            if (!string.IsNullOrEmpty(apiResponse))
            {

                Books = bookextensions.Deserialize(apiResponse);

               BookCount = Books.Count();
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }


    }
}