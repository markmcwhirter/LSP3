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


    private readonly ILogger<Book2Model> _logger = logger;

    private readonly AppSettings _appSettings = appSettings.Value;

    private readonly int? bookId;
    private readonly int? authorId;

    public async Task<IActionResult> OnGet(int? bookid, int? authorid)
    {
        int authorId = authorid != null ? authorid.Value : base.AuthorId;
        int bookId = bookid != null ? bookid.Value : 0;

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
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "data");
            Directory.CreateDirectory(uploadsFolder); // Create if not exists

            var ext = Path.GetExtension(File.FileName);

            string randomfile = UidGenerator.GenerateHtmlFriendlyUid(24);

            // Construct the full path for saving
            var filePath = Path.Combine(uploadsFolder, $"{bookId}_{randomfile}{ext}");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

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
        string path = "";
        bool iscopied = false;

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


                //string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                //path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Upload"));
                //using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                //{
                //    await file.CopyToAsync(filestream);
                //}
                iscopied = true;
            }
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }


        return Page();
    }

}
