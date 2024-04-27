using LSP3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class IndexModel : MasterModel
{
    [BindProperty]
    public AuthorDto? Author { get; set; }

    [BindProperty]
    public List<BookDto>? Books { get; set; }

    private readonly ILogger<IndexModel> _logger;

    private readonly AppSettings _appSettings;

    public IndexModel(IOptions<AppSettings> appSettings, ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor) : base( httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        HttpHelper helper = new HttpHelper();
        Extensions<AuthorDto> authorextensions = new Extensions<AuthorDto>();
        Extensions<List<BookDto>> bookextensions = new Extensions<List<BookDto>>();

        try
        {

            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            string apiResponse = await helper.Get(_appSettings.HostUrl + $"author/{base.Author.AuthorID}");

            if (!string.IsNullOrEmpty(apiResponse))
            {

                Author = authorextensions.Deserialize(apiResponse);
                #region Repeater
                //bookRepeater.DataSource = Books;
                //bookRepeater.DataBind();
                // try
                //{
                //    // Create a new instance of the Repeater control
                //    Repeater repeater = new Repeater();

                //    // Set the data source for the repeater control
                //    repeater.DataSource = Books;

                //    // Set the event handler for the ItemDataBound event
                //    repeater.ItemDataBound += Repeater_ItemDataBound;

                //    // Set the template for the repeater control
                //    repeater.ItemTemplate = new CustomRepeaterTemplate();

                //    // Bind the data to the repeater control
                //    repeater.DataBind();

                //    // Add the repeater control to the page
                //    // Replace "page" with the actual page object
                //    page.Controls.Add(repeater);
                //}
                //catch (Exception ex)
                //{
                //    // Log the error
                //    Console.WriteLine("An error occurred: " + ex.Message);
                //}
                #endregion

            }

            apiResponse = await helper.Get(_appSettings.HostUrl + $"book/author/{base.Author.AuthorID}");
            if (!string.IsNullOrEmpty(apiResponse))
            {

                Books = bookextensions.Deserialize(apiResponse);
                #region Repeater
                //bookRepeater.DataSource = Books;
                //bookRepeater.DataBind();
                // try
                //{
                //    // Create a new instance of the Repeater control
                //    Repeater repeater = new Repeater();

                //    // Set the data source for the repeater control
                //    repeater.DataSource = Books;

                //    // Set the event handler for the ItemDataBound event
                //    repeater.ItemDataBound += Repeater_ItemDataBound;

                //    // Set the template for the repeater control
                //    repeater.ItemTemplate = new CustomRepeaterTemplate();

                //    // Bind the data to the repeater control
                //    repeater.DataBind();

                //    // Add the repeater control to the page
                //    // Replace "page" with the actual page object
                //    page.Controls.Add(repeater);
                //}
                //catch (Exception ex)
                //{
                //    // Log the error
                //    Console.WriteLine("An error occurred: " + ex.Message);
                //}
                #endregion
                foreach (var b in Books)
                {
                    apiResponse = await helper.Get(_appSettings.HostUrl + $"sale/getsales/{b.BookID}");
                }
            }
            
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }

        return Page();

    }
}
