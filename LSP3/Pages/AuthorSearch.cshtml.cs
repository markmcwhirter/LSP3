using LSP3.Model;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace LSP3.Pages;

public class AuthorSearch : MasterModel
{
    private readonly ILogger<AuthorSearch> _logger;
    public AuthorSearchModel SearchTerm { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }

    public string SortOrder { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPages { get; private set; }

    public IList<AuthorListResultsModel> Results { get; set; }
    HttpHelper helper = new HttpHelper();
    Extensions<List<AuthorDto>> extensions = new Extensions<List<AuthorDto>>();
    private readonly AppSettings _appSettings;

    public AuthorSearch(IOptions<AppSettings> appSettings,ILogger<AuthorSearch> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    
    public async Task OnGetAsync(string LastName, string FirstName, string sortOrder, int? currentPage)
    {
        if (LastName == null && FirstName == null)
        {
            Results = new List<AuthorListResultsModel>();
            return;
        }

        SearchTerm = new AuthorSearchModel();
        SearchTerm.LastName = LastName == null ? " " : LastName; ;
        SearchTerm.FirstName = FirstName == null ? " " : FirstName;



        // Sorting
        SortOrder = sortOrder ?? "LastName"; // Default to sorting by last  name


        Extensions<List<AuthorListResultsModel>> listextensions = new Extensions<List<AuthorListResultsModel>>();
        try
        {
            SearchTerm.SortOrder = sortOrder ?? "LastName"; // Default to sorting by name

            var response = await helper.PostAsync(_appSettings.HostUrl + $"author/search", SearchTerm);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Results = JsonConvert.DeserializeObject<List<AuthorListResultsModel>>(responseString);


            // Pagination
            if (currentPage.HasValue)
            {
                CurrentPage = currentPage.Value;
            }
            TotalPages = (int)Math.Ceiling((decimal) Results.Count / PageSize);

            Results = Results.Skip((CurrentPage - 1) * PageSize)
                             .Take(PageSize).ToList();

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }

    }
}
