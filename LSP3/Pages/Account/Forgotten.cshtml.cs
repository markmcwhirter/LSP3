using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;

namespace LSP3.Pages.Account;

public class ForgottenModel : PageModel
{
	[BindProperty]
	public string? Email { get; set; }


	private readonly ILogger<ForgottenModel> _logger;
	private readonly AppSettings _appSettings;
	private readonly EmailService _emailService;
    private readonly IHttpClientFactory _httpClientFactory;

    public ForgottenModel(IHttpClientFactory httpClientFactory,IOptions<AppSettings> appSettings, ILogger<ForgottenModel> logger, IHttpContextAccessor httpContextAccessor,EmailService emailservice)
	{
        _httpClientFactory = httpClientFactory;
        _appSettings = appSettings.Value;
		_logger = logger;
		_emailService = emailservice;
	}


	public async Task<IActionResult> OnGet() 
	{
		try
		{
			string username = "";

			StringBuilder sb = new();

			HttpHelper helper = new();


			if (Request.Query.ContainsKey("username"))
				username = Request.Query["username"].ToString();
			else
				RedirectToPage("Login");

            var client = _httpClientFactory.CreateClient("apiClient");

            string email = await helper.GetFactoryAsync(client,$"{_appSettings.HostUrl}User/username/{username}");

			if (string.IsNullOrEmpty(email))
				return RedirectToPage("Login");

			sb.AppendLine("Thank you for joining LightSwitchPress's automated author system.");
			sb.AppendLine();
			sb.AppendLine("A request was made to send reset your password for LightSwitchPress. Please click on the following link to reset your password:");
			sb.AppendLine();

			string encrypted = Convert.ToBase64String(Encoding.UTF8.GetBytes(username));

			sb.AppendLine($"{_appSettings.HostUrl}/Account/Reset?username=" + encrypted);

			sb.AppendLine();

			sb.AppendLine("You didn't request your password?");
			sb.AppendLine();
			sb.AppendLine("Anyone can request this information, but only you will receive this email. This is done so that you can access your information from anywhere, using any computer. If you received this email but did not yourself request the information, then rest assured that the person making the request did not gain access to any of your information.");
			sb.AppendLine();
			sb.AppendLine("Regards,");
			sb.AppendLine("LightSwitchPress Team");
			sb.AppendLine();
			sb.AppendLine("Note: This message has been sent from an unattended email box.");


			_emailService.SendEmailAsync(email, "Password reset instructions", sb.ToString(), "");
		}
		catch(Exception ex)
        {
            _logger.LogError(ex.Message);
        }

		return RedirectToPage("Login");

	}

}


