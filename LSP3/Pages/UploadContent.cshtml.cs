using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

using System.Net;
using System.Text.Json;


namespace LSP3.Pages;

public class UploadContentModel : MasterModel

{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IOptions<AppSettings> appSettings;

    public UploadContentModel(IHttpClientFactory httpClientFactory,IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        this.httpClientFactory = httpClientFactory;
        this.appSettings = appSettings;
    }

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


    [BindProperty]
    public bool IsAdmin { get; set; }

    [BindProperty]
    public List<AuthorListItem> ListItems { get; set; }
 

    public async Task<IActionResult> OnGet(int? bookid, int? authorid)
    {
        SessionHelper sessionhelper = new();
        HttpHelper helper = new();


        List<AuthorListResults> authorlist = new();


        int authorId = authorid != null ? authorid.Value : base.AuthorId;
        int bookId = bookid != null ? bookid.Value : 0;

        TempData["AuthorId"] = authorId;
        TempData["BookId"] = bookId;

        // see if this is an admin 
        IsAdmin = sessionhelper.IsAdmin(_httpContextAccessor);

        if (IsAdmin)
        {

            ListItems = new List<AuthorListItem>();

            // retrieve author list
            var client = httpClientFactory.CreateClient("apiClient");
            var apiResponse = await helper.GetFactoryAsync(client, $"{appSettings.Value.HostUrl}author/getall");            

            if (!string.IsNullOrEmpty(apiResponse))
            {

                authorlist = JsonSerializer.Deserialize<List<AuthorListResults>>(apiResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            foreach (var a in authorlist)
            {
                ListItems.Add(new AuthorListItem { AuthorID = a.AuthorID, Name = $" Id - {a.AuthorID} Name: {a.LastName},{a.FirstName} Email: {a.EMail}" });
            }

        }


        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await UploadFile(fileInput1, "manuscript");
        await UploadFile(fileInput2, "cover");
        await UploadFile(fileInput4, "author");

        return RedirectToPage("/Index");
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

            if (!string.IsNullOrEmpty(_appSettings.ImageData) && !Directory.Exists(_appSettings.ImageData))
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
            var client = httpClientFactory.CreateClient("apiClient");
            var apiResponse = await httphelper.GetFactoryAsync(client, $"{appSettings.Value.HostUrl}book/{bookId}");

            if (!string.IsNullOrEmpty(apiResponse))
                Book = bookextensions.Deserialize(apiResponse);

            if (filetype == "manuscript")
                Book.Interior = filename;
            else if (filetype == "cover")
                Book.Cover = filename;
            else if (filetype == "author")
                Book.AuthorPhoto = filename;

            var response = await httphelper.PostFactoryAsync(client, $"{_appSettings.HostUrl}book/update", Book);
            var message = response.IsSuccessStatusCode ? "Upload saved successfully!" : "Upload could not be saved.";

        }

    }
}
