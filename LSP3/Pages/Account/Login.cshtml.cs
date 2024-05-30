using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace LSP3.Pages.Account;

public class LoginModel : PageModel
{
	[BindProperty]
	public string? Username { get; set; }

	[BindProperty]
	public string? Password { get; set; }

	private readonly ILogger<IndexModel> _logger;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly AppSettings _appSettings;

	static readonly char[] padding = { '=' };

	private bool IsAdmin = false;

	public LoginModel(IOptions<AppSettings> appSettings, ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor)
	{
		_appSettings = appSettings.Value;
		_logger = logger;
		_httpContextAccessor = httpContextAccessor;
	}

	public void OnGet() { }

	public async Task<IActionResult> OnPost()
	{
		SessionHelper helper = new();

		if (_httpContextAccessor.HttpContext != null)
			await OnGetauthor(Username, Password);

		if( helper.IsAuthenticated(_httpContextAccessor))
        {
            IsAdmin = helper.IsAdmin(_httpContextAccessor);
        }

		return IsAdmin ? RedirectToPage("/Admin") : RedirectToPage("/Index");
	}

	public async Task OnGetauthor(string? username, string? password)
	{

		HttpHelper helper = new();
		SessionHelper sessionHelper = new SessionHelper();

		if (username == null || password == null) return;

		try
		{
			byte[]? arrencrypted = await new EncryptionService().EncryptAsync(password);
			var encrypted = Convert.ToBase64String(arrencrypted).TrimEnd(padding).Replace('+', '-').Replace('/', '_');


			string apiResponse = await helper.Get(_appSettings.HostUrl + $"author/{username}/{encrypted}");


			if (apiResponse != null)
			{
				AuthorDto author = new Extensions<AuthorDto>().Deserialize(apiResponse);
				if (author.AuthorID == 0)
				{
					Username = "";
					return;
				}

				sessionHelper.SetSessionString(_httpContextAccessor, "AuthorId", author.AuthorID.ToString());
                sessionHelper.SetSessionString(_httpContextAccessor, "Authenticated", "true");

				var isAdmin = author.Admin == "1" ? "true" : "false";
                sessionHelper.SetSessionString(_httpContextAccessor, "Admin", isAdmin);


            }

		}
		catch (Exception ex)
		{
			_logger.LogError($"Error: {ex.Message} {ex.InnerException} {ex.StackTrace}");
		}

	}
}


