using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Text.Json;

using System.Net;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace LSP3.Pages;

public class AddBookImage : PageModel
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

    private readonly ILogger<AddBookImage> _logger;

    private readonly AppSettings _appSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddBookImage(IOptions<AppSettings> appSettings, ILogger<AddBookImage> logger, IHttpContextAccessor httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IActionResult> OnGet()
    {
        ImageType = System.Web.HttpUtility.HtmlDecode(Request.Query["type"].ToString().ToLower());
        BookId = int.Parse(System.Web.HttpUtility.HtmlDecode(Request.Query["bookid"].ToString()));

        string apiResponse;

        HttpHelper helper = new();
        SessionHelper sessionHelper = new();
        Extensions<AuthorDto> authorextensions = new();
        Extensions<BookDto> bookextensions = new();

        try
        {

            if (!sessionHelper.IsAuthenticated(_httpContextAccessor))
                return Redirect("/Account/Login");


        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

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


        try
        {
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

            var apiResponse = await httphelper.Get(_appSettings.HostUrl + $"book/{BookId}");

            if (!string.IsNullOrEmpty(apiResponse))
                Book = bookextensions.Deserialize(apiResponse);


            var image = System.Web.HttpUtility.HtmlDecode(Request.Query["type"].ToString().ToLower());

            if (image == "cover")
                Book.Cover = bookfilename;
            else if(image == "interior")
                Book.Interior = bookfilename;
            else if(image == "author")
                Book.AuthorPhoto = bookfilename;

            string jsonString = JsonSerializer.Serialize(Book);

            _ = await httphelper.PostAsync(_appSettings.HostUrl + "book/update", Book);


            SessionHelper helper = new();

            bool IsAdmin = false;

            if (helper.IsAuthenticated(_httpContextAccessor))
            {
                IsAdmin = helper.IsAdmin(_httpContextAccessor);
            }
            return IsAdmin ? RedirectToPage("/Admin") : RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            Status = "File could not be uploaded. Error: " + ex.Message;
        }

        return Page();
    }
}
