using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class BookListModel(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, ILogger<BookListModel> logger, IHttpContextAccessor httpContextAccessor) : MasterModel(httpContextAccessor)
{
    [BindProperty]
    public List<BookDto>? Books { get; set; }

    private readonly AppSettings _appSettings = appSettings.Value;

    public async Task<IActionResult> OnGet()
    {

        HttpHelper httphelper = new();
        Extensions<List<BookDto>> extensions = new();


        if (!base.IsAuthenticated)
            return Redirect("/Account/Login");

        if (!base.IsAdmin)
            return Redirect("/Index");

        var client = httpClientFactory.CreateClient("apiClient");
        var apiResponse = await httphelper.GetFactoryAsync(client, $"{_appSettings.HostUrl}author/{Author.AuthorID}");

        Books = extensions.Deserialize(apiResponse);

        return Page();


    }
}
