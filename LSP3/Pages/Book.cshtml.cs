using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.Web;

namespace LSP3.Pages;

public class BookModel : MasterModel
{
    public bool IsReadOnly { get; set; }

    [BindProperty]
    public BookDto? Book { get; set; }

    [BindProperty]
    public int? AuthorId { get; set; }

    [BindProperty]
    public string? Referrer { get; set; }


    private readonly ILogger<BookModel> _logger;

    private readonly AppSettings _appSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private IWebHostEnvironment _environment;
    [BindProperty]
    public IFormFile File { get; set; }
    public string UploadMessage { get; set; }

    public BookModel(IOptions<AppSettings> appSettings, ILogger<BookModel> logger, IHttpContextAccessor httpContextAccessor, 
        IWebHostEnvironment environment) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _environment = environment;
    }

    public async Task<IActionResult> OnPostHandlePostRequest(IFormFile file)
    {
        if (File != null && File.Length > 0)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "data"); // "uploads" folder within wwwroot
            var fileName = Path.GetFileName(File.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            Directory.CreateDirectory(uploadsFolder); // Ensure folder exists
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await File.CopyToAsync(fileStream);
            }

            UploadMessage = $"File '{fileName}' uploaded successfully.";
        }
        else
        {
            UploadMessage = "Please select a file.";
        }
        return new JsonResult(new { message = "" });
    }

    public async Task<IActionResult> OnPostAsync(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "data"); // "uploads" folder within wwwroot
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            Directory.CreateDirectory(uploadsFolder); // Ensure folder exists
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await File.CopyToAsync(fileStream);
            }

            UploadMessage = $"File '{fileName}' uploaded successfully.";
        }
        else
        {
            UploadMessage = "Please select a file.";
        }
        return Page();
    }

    public async Task<IActionResult> OnGet(int? bookid, int? authorid)
    {


        try
        {
            HttpHelper helper = new();
            SessionHelper sessionHelper = new();
            Extensions<BookDto> bookextensions = new Extensions<BookDto>();


            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            IsReadOnly = false;
            Book = new BookDto();


            Referrer = GetReferrer().AbsoluteUri;

            if (Request.Query.ContainsKey("authorid"))
            {
                AuthorId = authorid;
                Book.AuthorID = authorid ?? 0;
            }

            if (Request.Query.ContainsKey("bookid"))
            {
                IsReadOnly = true;

                var apiResponse = await helper.Get(_appSettings.HostUrl + $"book/{bookid}");

                if (!string.IsNullOrEmpty(apiResponse))
                    Book = bookextensions.Deserialize(apiResponse);
            }


        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }

        return Page();
    }
    private Uri GetReferrer()
    {
        var header = _httpContextAccessor.HttpContext.Request.GetTypedHeaders();
        return header.Referer;
    }
}
