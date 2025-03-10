using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

using System.Text.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net;


namespace LSP3.Pages;

public class BookModel(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, ILogger<BookModel> logger, IHttpContextAccessor httpContextAccessor,
    IWebHostEnvironment environment) : MasterModel(httpContextAccessor)
{
    public bool IsReadOnly { get; set; }

    [BindProperty]
    public BookDto? Book { get; set; }

    [BindProperty]
    public int? SelectedValue { get; set; }

    [BindProperty]
    public string? Referrer { get; set; }

    [BindProperty]
    public bool IsAdmin { get; set; }

    [BindProperty]
    public List<AuthorListItem> ListItems { get; set; }

    [BindProperty]
    public IFormFile File { get; set; }


    private readonly ILogger<BookModel> _logger = logger;

    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly IWebHostEnvironment _environment = environment;


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

    public async Task<IActionResult> OnGet(int? bookid, int authorid = 0)
    {

        try
        {
            HttpHelper httphelper = new();
            SessionHelper sessionhelper = new();

            Extensions<BookDto> bookextensions = new();
            Extensions<AuthorListResultsModel> authorextensions = new();

            List<AuthorListResults> authorlist = new();

            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            IsReadOnly = false;
            Book = new BookDto();
            string apiResponse = "";

            var client = httpClientFactory.CreateClient("apiClient");

            // see if this is an admin 
            IsAdmin = sessionhelper.IsAdmin(_httpContextAccessor);

            if (IsAdmin && authorid != 0)
            {
                ListItems = new List<AuthorListItem>();

                apiResponse = await httphelper.GetFactoryAsync(client, $"{_appSettings.HostUrl}author/{authorid}");

                if (!string.IsNullOrEmpty(apiResponse))
                {

                    AuthorListResults? author = System.Text.Json.JsonSerializer.Deserialize<AuthorListResults>(apiResponse, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    ListItems.Add(new AuthorListItem { AuthorID = author.AuthorID, Name = $" Id - {author.AuthorID} Name: {author.LastName},{author.FirstName} Email: {author.EMail}" });

                }
            }
            else if (IsAdmin && authorid == 0)
            {

                ListItems = new List<AuthorListItem>();

                // retrieve author list
                apiResponse = await httphelper.GetFactoryAsync(client, $"{_appSettings.HostUrl}author/getall");

                if (!string.IsNullOrEmpty(apiResponse))
                {

                    authorlist = System.Text.Json.JsonSerializer.Deserialize<List<AuthorListResults>>(apiResponse, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                }
                foreach (var a in authorlist)
                {
                    ListItems.Add(new AuthorListItem { AuthorID = a.AuthorID, Name = $" Id - {a.AuthorID} Name: {a.LastName},{a.FirstName} Email: {a.EMail}" });
                }

            }

            if (!IsAdmin && Request.Query.ContainsKey("authorid"))
            {
                AuthorId = authorid;
                Book.AuthorID = authorid;
            }

            if (Request.Query.ContainsKey("bookid"))
            {
                IsReadOnly = true;

                apiResponse = await httphelper.GetFactoryAsync(client, $"{_appSettings.HostUrl}book/{bookid}");

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
        HttpHelper httphelper = new();


        if (!base.IsAuthenticated)
            return Redirect("/Account/Login");

        var client = httpClientFactory.CreateClient("apiClient");

        var response = await httphelper.PostFactoryAsync(client, $"{_appSettings.HostUrl}book/update", Book);

        var message = response.IsSuccessStatusCode ? "Book saved successfully!" : "Book could not be saved.";


        return RedirectToPage("/Index");
    }
}

public class AuthorListItem
{
    public int AuthorID { get; set; }
    public string Name { get; set; }
}