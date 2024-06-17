using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.Web;

namespace LSP3.Pages;

public class BookModel(IOptions<AppSettings> appSettings, ILogger<BookModel> logger, IHttpContextAccessor httpContextAccessor,
    IWebHostEnvironment environment) : MasterModel(httpContextAccessor)
{
    public bool IsReadOnly { get; set; }

    [BindProperty]
    public BookDto? Book { get; set; }

    [BindProperty]
    public int? AuthorId { get; set; }

    [BindProperty]
    public string? Referrer { get; set; }


    private readonly ILogger<BookModel> _logger = logger;

    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly IWebHostEnvironment _environment = environment;
    [BindProperty]
    public IFormFile File { get; set; }
    public string UploadMessage { get; set; }

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
            Extensions<BookDto> bookextensions = new();


            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            IsReadOnly = false;
            Book = new BookDto();


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
            _logger.LogError(ex.Message);
        }

        return Page();
    }
}
