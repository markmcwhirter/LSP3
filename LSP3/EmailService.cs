using LSP3.Model;


using System.Net;

using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;


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



    public async Task SendEmailAsync( string fromEmail, string subject, string plainTextContent, string htmlContent)
    {
        string apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY") ?? string.Empty;
        string envfrom = Environment.GetEnvironmentVariable("SMTP_FROM") ?? string.Empty;
        string envto = Environment.GetEnvironmentVariable("SMTP_TO") ?? string.Empty;
        int envport = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT").ToString() ?? string.Empty);


        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(envfrom, "");
        var to = new EmailAddress(envto, "");
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        try
        {
            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                Console.WriteLine("Email sent successfully!");
            }
            else
            {
                Console.WriteLine("Error sending email. Status Code: " + response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error sending email: " + ex.Message);
        }
    }

    //public async Task SendEmailAsync(string recipientEmail, string subject, string body)
    //{
    //    var emailMessage = new MimeMessage();

    //    emailMessage.From.Add(new MailboxAddress("Light Switch Press", _appSettings.Value.SmtpUserName));
    //    emailMessage.To.Add(new MailboxAddress("", recipientEmail));
    //    emailMessage.Subject = subject;
    //    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
    //    {
    //        Text = body
    //    };

    //    using (var client = new SmtpClient())
    //    {
    //        int port = 465;
    //        if (!string.IsNullOrEmpty(_appSettings.Value.SmtpPort))
    //            port = int.Parse(_appSettings.Value.SmtpPort);

    //        await client.ConnectAsync(_appSettings.Value.SmtpServer, port, true);
    //        await client.AuthenticateAsync(_appSettings.Value.SmtpUserName, _appSettings.Value.SmtpPassword);
    //        await client.SendAsync(emailMessage);
    //        await client.DisconnectAsync(true);
    //    }
    //}



    //static async Task Main(string[] args)
    //{
    //    await SendEmailAsync("your_sendgrid_api_key",
    //                         "sender@example.com",
    //                         "recipient@example.com",
    //                         "Email Subject",
    //                         "Plain text content",
    //                         "<b>HTML content</b>");
    //}

}

