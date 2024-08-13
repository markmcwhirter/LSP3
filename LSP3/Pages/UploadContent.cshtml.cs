using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.Text.Json;


namespace LSP3.Pages;

public class UploadContentModel(IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor) : MasterModel(httpContextAccessor)

{

    [BindProperty]
    public string Status { get; set; }


    [BindProperty]
    public IFormFile fileInput1 { get; set; }

    [BindProperty]
    public IFormFile fileInput2 { get; set; }

    [BindProperty]
    public IFormFile fileInput3 { get; set; }

    [BindProperty]
    public IFormFile fileInput4 { get; set; }


    private int? bookId;
    private int? authorId;

    public async Task<IActionResult> OnGet(int? bookid, int? authorid)
    {
        int authorId = authorid != null ? authorid.Value : base.AuthorId;
        int bookId = bookid != null ? bookid.Value : 0;

        TempData["AuthorId"] = authorId;
        TempData["BookId"] = bookId;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        SessionHelper helper = new();

        await UploadFile(fileInput1, "manuscript");
        await UploadFile(fileInput2, "cover");
        await UploadFile(fileInput4, "author");

        if (helper.IsAuthenticated(_httpContextAccessor))
        {
            IsAdmin = helper.IsAdmin(_httpContextAccessor);
        }

        return IsAdmin ? RedirectToPage("/Admin") : RedirectToPage("/Index");

        return Page();
    }

    private async Task UploadFile(IFormFile inputFile, string filetype)
    {
        HttpHelper httphelper = new();
        Extensions<BookDto> bookextensions = new();
        BookDto? Book = new();

        if (inputFile != null) // manuscript upload
        {
            var _appSettings = appSettings.Value;

            int bookId = TempData["BookId"] != null ? Convert.ToInt32(TempData["BookId"]) : 0;
            int authorId = TempData["AuthorId"] != null ? Convert.ToInt32(TempData["AuthorId"]) : 0;

            if (!Directory.Exists(_appSettings.ImageData))
            {
                Directory.CreateDirectory(_appSettings.ImageData);
            }


            string randomfile = UidGenerator.GenerateHtmlFriendlyUid(24);
            var ext = Path.GetExtension(inputFile.FileName);
            var filename = $"{bookId}_{randomfile}{ext}";
            var filePath = Path.Combine(_appSettings.ImageData, filename);

            // Save the file to the server
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await inputFile.CopyToAsync(fileStream);
            }

            // update document path in the database
            var apiResponse = await httphelper.Get(_appSettings.HostUrl + $"book/{bookId}");

            if (!string.IsNullOrEmpty(apiResponse))
                Book = bookextensions.Deserialize(apiResponse);

            if (filetype == "manuscript")
                Book.Interior = filename;
            else if (filetype == "cover")
                Book.Cover = filename;
            else if (filetype == "author")
                Book.AuthorPhoto = filename;

            string jsonString = JsonSerializer.Serialize(Book);
            _ = await httphelper.PostAsync(_appSettings.HostUrl + "book/update", Book);
        }

    }
}
