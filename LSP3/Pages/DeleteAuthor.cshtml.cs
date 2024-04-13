using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class DeleteAuthor : MasterModel
{
    private readonly ILogger<DeleteAuthor> _logger;

    public IList<AuthorListResultsModel> Results { get; set; }

    HttpHelper helper = new HttpHelper();
    Extensions<List<AuthorDto>> extensions = new Extensions<List<AuthorDto>>();
    private readonly AppSettings _appSettings;

    public DeleteAuthor(IOptions<AppSettings> appSettings, ILogger<DeleteAuthor> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }


    public IActionResult OnGet()
    {
        return RedirectToPage("/AuthorSearch");
    }
}
