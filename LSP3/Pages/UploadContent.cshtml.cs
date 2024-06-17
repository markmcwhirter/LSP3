using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace LSP3.Pages;


public class UploadContentModel : MasterModel
{
    [BindProperty]
    public string Status { get; set; }

    [BindProperty]
    public List<BookListSummaryModel> Books { get; set; }

    [BindProperty]
    public IEnumerable<SelectListItem> BookOptions { get; set; }

    [BindProperty]
    public IEnumerable<SelectListItem> ContentTypes { get; set; }


    [BindProperty]
    public int? AuthorId { get; set; }

    [BindProperty]
    public int? SelectedBookId { get; set; }

    [BindProperty]
    public string? SelectedContentType { get; set; }

    [BindProperty]
    [Required]
    public IFormFile File { get; set; }


    private readonly ILogger<UploadContentModel> _logger;

    private readonly AppSettings _appSettings;

    public UploadContentModel(IOptions<AppSettings> appSettings, ILogger<UploadContentModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet(int? bookid, int? authorid)
    {
        int id = authorid != null ? authorid.Value : base.AuthorId;


        try
        {
            HttpHelper helper = new();

            BookOptions = [];
            ContentTypes =
            [
                new() { Value = "cover", Text = "Cover Image" },
                new() { Value = "interior", Text = "Interior Image" },
                new() { Value = "author", Text = "Author Photo" },
                new() { Value = "doc", Text = "Book Document" }
            ];
            Status = "";


            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            Books = [];

            var apiResponse = await helper.Get(_appSettings.HostUrl + $"book/getidbyauthor/{id}");

            if (!string.IsNullOrEmpty(apiResponse))
                Books = JsonSerializer.Deserialize<List<BookListSummaryModel>>(apiResponse);

            foreach (var b in Books)
            {
                b.bookTitle = $"{b.bookID,6} - {b.bookTitle,-25}";
            }

            BookOptions = Books.Select(b => new SelectListItem { Value = b.bookID.ToString(), Text = b.bookTitle });



        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        return Page();
    }



    public async Task<IActionResult> OnPostAsync()
    {
        HttpHelper helper = new();

        if (!ModelState.IsValid)
        {
            return Redirect($"/UploadContent?authorid={base.AuthorId}");
        }

        // Get the upload directory (e.g., wwwroot/uploads)
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "data");
        Directory.CreateDirectory(uploadsFolder); // Create if not exists

        var ext = Path.GetExtension(File.FileName);

        string randomfile = UidGenerator.GenerateHtmlFriendlyUid(24);

        // Construct the full path for saving
        var filePath = Path.Combine(uploadsFolder, $"{SelectedBookId}_{randomfile}{ext}");

        // Save the file to the server
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await File.CopyToAsync(fileStream);
        }

        var ctype = SelectedContentType.Trim().ToLower();

        if (ctype == "doc")
        {
            Status = "File Uploaded";
            return Redirect($"/UploadContent?authorid={base.AuthorId}");
        }


        var response = await helper.Get(_appSettings.HostUrl + $"book/{SelectedBookId}");

        BookDto tmpbook = new();

        if (!string.IsNullOrEmpty(response))
            tmpbook = new Extensions<BookDto>().Deserialize(response);

        var tmpfile = Path.GetFileName(filePath);



        switch (ctype)
        {
            case "cover":
                tmpbook.Cover = tmpfile;
                break;
            case "interior":
                tmpbook.Interior = tmpfile;
                break;
            case "author":
                tmpbook.AuthorPhoto = tmpfile;
                break;
        }

        _ = await helper.PostAsync(_appSettings.HostUrl + $"book/update", tmpbook);

        Status = "File Uploaded";

        return  Redirect($"/UploadContent?authorid={base.AuthorId}"); 
    }
   

}
