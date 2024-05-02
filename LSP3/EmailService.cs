using LSP3.Model;

using MailKit.Net.Smtp;

using Microsoft.Extensions.Options;

using MimeKit;
using Microsoft.Extensions.Configuration;

namespace LSP3;



public class EmailService
{
	private readonly ILogger<EmailService> _logger;
	private readonly IOptions<AppSettings> _appSettings;

	public EmailService(IOptions<AppSettings> appSettings, ILogger<EmailService> logger)
	{
		_appSettings = appSettings;
		_logger = logger;

	}

	public async Task SendEmailAsync(string recipientEmail, string subject, string body)
	{
		var emailMessage = new MimeMessage();

		emailMessage.From.Add(new MailboxAddress("Light Switch Press", _appSettings.Value.SmtpUserName));
		emailMessage.To.Add(new MailboxAddress("", recipientEmail));
		emailMessage.Subject = subject;
		emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
		{
			Text = body
		};

		using (var client = new SmtpClient())
		{
			int port = 465;
			if (!string.IsNullOrEmpty(_appSettings.Value.SmtpPort))
				port = int.Parse(_appSettings.Value.SmtpPort);

			await client.ConnectAsync(_appSettings.Value.SmtpServer,port, true);
			await client.AuthenticateAsync(_appSettings.Value.SmtpUserName, _appSettings.Value.SmtpPassword);
			await client.SendAsync(emailMessage);
			await client.DisconnectAsync(true);
		}
	}
}
