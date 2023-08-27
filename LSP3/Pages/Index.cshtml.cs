using LSP3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Text.Json;

namespace LSP3.Pages;

public class IndexModel : MasterModel
{
    [BindProperty]
    public AuthorDto Author { get; set; }

    [BindProperty]
    public List<BookDto> Books { get; set; }


    public IndexModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
    {
    }

    public async Task<IActionResult> OnGet()
    {

        try
        {

            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");


           // var session = _httpContextAccessor.HttpContext.Session.GetString("userSession");

            //if (IsAuthenticated)
            //{
            //    ViewBag.Link = "Welcome " + Author.FirstName + "! " + "[<a href=\"Account/Logout.aspx\" ' class=\"loginDisplay\">Logout</a>]";
            //}
            //else
            //{
            //    ViewBag.Link = "[<a href=\"Account/Login.aspx\">Login</a>]";
            //}


            //if ( session != null )
            //    Author = JsonSerializer.Deserialize<AuthorDto>(session, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            string apiResponse = await new HttpHelper().Get($"https://localhost:7253/api/book/author/{base.Author.AuthorID}");
            if (!string.IsNullOrEmpty(apiResponse))
            {
                Books = JsonSerializer.Deserialize<List<BookDto>>(apiResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
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

            }
        }
        catch (Exception ex)
        {
            string error = ex.Message;
        }

        return Page();

    }




    //public void OnPost()
    //{
    //    Console.WriteLine("Received a post request.");
    //}

}
