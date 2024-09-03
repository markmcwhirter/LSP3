using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace LSP3.Pages;

public class Book2Model(IOptions<AppSettings> appSettings, ILogger<Book2Model> logger, IHttpContextAccessor httpContextAccessor) : MasterModel(httpContextAccessor)

{

    [BindProperty]
    public string Status { get; set; }


    [BindProperty]
    public IFormFile File { get; set; }

    private readonly AppSettings _appSettings = appSettings.Value;

    private int? bookId = 0;

    public async Task<IActionResult> OnGet(int? bookid, int? authorid)
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (File == null || File.Length == 0)
        {
            Status = "No file selected.";
            return Page();
        }


        try
        {
            if ( !string.IsNullOrEmpty(_appSettings.ImageData) && !Directory.Exists(_appSettings.ImageData))
            {
                Directory.CreateDirectory(_appSettings.ImageData);
            }

            var ext = Path.GetExtension(File.FileName);

            string randomfile = UidGenerator.GenerateHtmlFriendlyUid(24);

            // Construct the full path for saving
            var filePath = Path.Combine(_appSettings.ImageData, $"{bookId}_{randomfile}{ext}");


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await File.CopyToAsync(stream);
            }

            Status = "File uploaded successfully!";
        }
        catch (Exception ex)
        {
            Status = "File could not be uploaded. Error: " + ex.Message;
        }

        return Page();
    }

public async Task<ActionResult> FileUpload(IFormFile file)
    {
        try
        {
            if (file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "data");
                Directory.CreateDirectory(uploadsFolder); // Create if not exists

                var ext = Path.GetExtension(File.FileName);

                string randomfile = UidGenerator.GenerateHtmlFriendlyUid(24);

                // Construct the full path for saving
                var filePath = Path.Combine(uploadsFolder, $"{bookId}_{randomfile}{ext}");

                // Save the file to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await File.CopyToAsync(fileStream);
                }

            }
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }


        return Page();
    }

}
