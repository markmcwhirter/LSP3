using LSP3.Model;

using Microsoft.Extensions.Options;

using System.Net;
using System.Net.Mail;


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



    public async Task SendEmailAsync( string toEmail, string subject, string plainTextContent, string htmlContent)
    {

        try
        {


            var smtpClient = new SmtpClient(_appSettings.Value.SmtpServer)
            {
                Port = int.Parse(_appSettings.Value.SmtpPort),
                Credentials = new NetworkCredential(_appSettings.Value.SmtpUser, _appSettings.Value.SmtpPassword),
                EnableSsl = false,
            };

            smtpClient.Send(_appSettings.Value.SmtpFromAddress, toEmail, subject, plainTextContent);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending email - {ex.Message}{ex.StackTrace}");
        }


    }

}

