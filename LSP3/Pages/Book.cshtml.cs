using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.Text.Json;


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
            Response.Redirect($"Book2?BookID={Book.BookID}");
        }
        else
        {
            UploadMessage = "Please select a file.";
        }
        return new JsonResult(new { message = "" });

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

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            HttpHelper helper = new();
            Extensions<BookDto> bookextensions = new();

            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            Book.DateCreated = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");

            string jsonString = JsonSerializer.Serialize(Book);

            var apiResponse = await helper.PostAsync(_appSettings.HostUrl + "book/update", jsonString);

            if (Book.BookID > 0)
                UploadMessage = "Book saved successfully!";
            else
                UploadMessage = "Book could not be saved.";

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        return Page();
    }
}
