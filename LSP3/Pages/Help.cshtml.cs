using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;


namespace LSP3.Pages
{
    public class HelpModel : MasterModel
    {
        [BindProperty]
        public string From { get; set; }

        [BindProperty]
        public string FromEmail{ get; set; }

        private IConfiguration Configuration;
        
        private readonly ILogger<IndexModel> _logger;
        public HelpModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
        }


        public async Task<IActionResult> OnGet()
        {
            HttpHelper helper = new HttpHelper();

            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            From = base.CurrentUser;
            FromEmail = base.Author.Email;

            return Page();
        }
    }

    public class ContactFormModel
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
    }
}
