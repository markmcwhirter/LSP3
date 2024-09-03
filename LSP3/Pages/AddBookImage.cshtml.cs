using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

using System.Text.Json;

namespace LSP3.Pages;

public class AddBookImage(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, ILogger<AddBookImage> logger, IHttpContextAccessor httpContextAccessor) : PageModel
{
    [BindProperty]
    public string Status { get; set; }


    [BindProperty]
    public string ImageType { get; set; }


    [BindProperty]
    public int BookId { get; set; }

    [BindProperty]
    public IFormFile File { get; set; }

    [BindProperty]
    public AuthorDto? Author { get; set; }

    [BindProperty]
    public BookDto? Book { get; set; }


    [BindProperty]
    public string? Text { get; set; }

    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public IActionResult OnGet()
    {
        ImageType = System.Web.HttpUtility.HtmlDecode(Request.Query["type"].ToString().ToLower());
        BookId = int.Parse(System.Web.HttpUtility.HtmlDecode(Request.Query["bookid"].ToString()));

        SessionHelper sessionHelper = new();

        if (!sessionHelper.IsAuthenticated(_httpContextAccessor))
            return Redirect("/Account/Login");

        return Page();


    }

    public async Task<IActionResult> OnPostAsync()
    {
        HttpHelper httphelper = new();
        Extensions<BookDto> bookextensions = new();


        if (File == null || File.Length == 0)
        {
            Status = "No file selected.";
            return Page();
        }

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "data");
        Directory.CreateDirectory(uploadsFolder); // Create if not exists

        var ext = Path.GetExtension(File.FileName);

        string randomfile = UidGenerator.GenerateHtmlFriendlyUid(24);

        // Construct the full path for saving
        var bookfilename = $"{BookId}_{randomfile}{ext}";
        var filePath = Path.Combine(uploadsFolder, bookfilename);

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await File.CopyToAsync(stream);
        }

        Status = "File uploaded successfully!";

        var client = _httpClientFactory.CreateClient("apiClient");

        var apiResponse = await httphelper.GetFactoryAsync(client, $"{_appSettings.HostUrl}book/{BookId}");

        if (!string.IsNullOrEmpty(apiResponse))
            Book = bookextensions.Deserialize(apiResponse);


        var image = System.Web.HttpUtility.HtmlDecode(Request.Query["type"].ToString().ToLower());

        if (image == "cover")
            Book.Cover = bookfilename;
        else if (image == "interior")
            Book.Interior = bookfilename;
        else if (image == "author")
            Book.AuthorPhoto = bookfilename;

        string jsonString = JsonSerializer.Serialize(Book);

        _ = await httphelper.PostFactoryAsync(client, $"{_appSettings.HostUrl}book/update", Book);

        return RedirectToPage("/Index");

    }
}
