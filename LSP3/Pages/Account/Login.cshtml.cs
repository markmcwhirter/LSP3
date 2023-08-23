using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LSP3.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string? Username { get; set; }

        [BindProperty]
        public string? Password { get; set; }

		private readonly ILogger<IndexModel> _logger;
		private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

			public void OnGet()
        {
            /*
            if (Cookies["LoginDetail"] != null)
            {
                if (Response.Cookies["LoginDetail"]["Username"] != null)
                {
                    UserName.Text = Response.Cookies["LoginDetail"]["Username"].ToString();
                    Password.Text = Response.Cookies["LoginDetail"]["Password"].ToString();
                }
            }
            */
        }

        public IActionResult OnPost()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString("Authenticated", "true");
                _httpContextAccessor.HttpContext.Session.SetString("currentuser", Username);
            }

			return RedirectToPage("/Index");
        }
    }
}
