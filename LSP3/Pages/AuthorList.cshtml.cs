using LSP3.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LSP3.Pages;

public class AuthorListModel : MasterModel
{
    [BindProperty]
    public List<AuthorDto> AuthorList { get; set; }

    [BindProperty]
    public List<BookDto> Books { get; set; }


    public AuthorListModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
    {
    }

    public async Task<IActionResult> OnGet()
    {
        HttpHelper helper = new HttpHelper();
        Extensions<List<AuthorDto>> extensions = new Extensions<List<AuthorDto>>();

        try
        {

            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            string apiResponse = await helper.Get($"https://localhost:7253/api/author");
            AuthorList = extensions.Deserialize(apiResponse);
            foreach( var author in AuthorList )
            {
                author.Email = string.IsNullOrEmpty(author.Email) ? string.Empty : author.Email;
                //author.Email = author.Email.Replace("@", "_");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }

        return Page();

    }


}
