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

        try
        {

            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            string apiResponse = await new HttpHelper().Get($"https://localhost:7253/api/author");
            if (!string.IsNullOrEmpty(apiResponse))
            {
                AuthorList = JsonSerializer.Deserialize<List<AuthorDto>>(apiResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }

        return Page();

    }


}
