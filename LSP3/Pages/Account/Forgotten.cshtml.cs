using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

using System.Net;
using System.Net.Mail;
using System.Text;

namespace LSP3.Pages.Account;

public class ForgottenModel : PageModel
{
	[BindProperty]
	public string? Email { get; set; }


	private readonly ILogger<ForgottenModel> _logger;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly AppSettings _appSettings;
	private readonly EmailService _emailService;

	static readonly char[] padding = { '=' };


	public ForgottenModel(IOptions<AppSettings> appSettings, ILogger<ForgottenModel> logger, IHttpContextAccessor httpContextAccessor,EmailService emailservice)
	{
		_appSettings = appSettings.Value;
		_logger = logger;
		_httpContextAccessor = httpContextAccessor;
		_emailService = emailservice;
	}

	//public void OnGet() { }

	//public async Task<IActionResult> OnPost()
	public async Task<IActionResult> OnGet() 
	{
		string username = "";

		StringBuilder sb = new();

		HttpHelper helper = new();


        if (Request.Query.ContainsKey("username"))
            username = Request.Query["username"].ToString();
		else
			RedirectToPage("Login");

		string email = await helper.Get(_appSettings.HostUrl + $"User/username/{username}");

		sb.AppendLine("Thank you for joining LightSwitchPress's automated author system.");
		sb.AppendLine();
		sb.AppendLine("A request was made to send reset your password for LightSwitchPress. Please click on the following link to reset your password:");
		sb.AppendLine();
		//string encrypted = EncryptionHelper.Encrypt(username);
		string encrypted = Convert.ToBase64String(Encoding.UTF8.GetBytes(username));

		sb.AppendLine("http://143.110.232.75/Account/Reset?username=" + encrypted);

		sb.AppendLine();

		sb.AppendLine("You didn't request your password?");
		sb.AppendLine();
		sb.AppendLine("Anyone can request this information, but only you will receive this email. This is done so that you can access your information from anywhere, using any computer. If you received this email but did not yourself request the information, then rest assured that the person making the request did not gain access to any of your information.");
		sb.AppendLine();
		sb.AppendLine("Regards,");
		sb.AppendLine("LightSwitchPress Team");
		sb.AppendLine();
		sb.AppendLine("Note: This message has been sent from an unattended email box.");


		await _emailService.SendEmailAsync(email, "Password reset instructions",sb.ToString(),"");

		return RedirectToPage("Login");

	}

}


