using LSP3.Model;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace LSP3.Pages;

public class AuthorSearch : MasterModel
{
    private readonly ILogger<AuthorSearch> _logger;
    public AuthorSearchModel? SearchTerm { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }

    public string? SortOrder { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPages { get; private set; }

    public IList<AuthorListResultsModel>? Results { get; set; }
    readonly HttpHelper helper = new();
    readonly Extensions<List<AuthorDto>> extensions = new();
    private readonly AppSettings _appSettings;

    public AuthorSearch(IOptions<AppSettings> appSettings,ILogger<AuthorSearch> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public async Task OnGetAsync(string lastName, string firstName, string sortOrder, int? currentPage)
    {
        SortOrder = sortOrder;
        CurrentPage = currentPage ?? 1;
        LastName = lastName;
        FirstName = firstName;

        if (lastName == null && firstName == null)
        {
            Results = new List<AuthorListResultsModel>();
            return;
        }

        SearchTerm = new AuthorSearchModel
        {
            LastName = lastName ?? " ",
            FirstName = firstName ?? " "
        };



        // Sorting
        SortOrder = sortOrder ?? "LastName"; // Default to sorting by last  name


        Extensions<List<AuthorListResultsModel>> listextensions = new();
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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            TotalPages = (int)Math.Ceiling((decimal) Results.Count / PageSize);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            Results = Results.Skip((CurrentPage - 1) * PageSize)
                             .Take(PageSize).ToList();

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }

    }
}
