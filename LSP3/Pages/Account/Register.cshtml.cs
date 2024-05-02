using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LSP3.Pages.Account
{
    public class RegisterModel : PageModel
    {
        public void OnGet()
        {
            _ = "test";
        }
        public IResult OnPost()
        {
            _ = "test";
            return (IResult)Page();
        }
    }
}
