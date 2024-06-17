using LSP3.Model;

using Microsoft.Extensions.Options;

using System.Net;
using System.Net.Mail;


namespace LSP3;



public class EmailService(IOptions<AppSettings> appSettings, ILogger<EmailService> logger)
{
    private readonly ILogger<EmailService> _logger = logger;
    private readonly IOptions<AppSettings> _appSettings = appSettings;

    public void  SendEmailAsync( string toEmail, string subject, string plainTextContent, string htmlContent)
    {

        try
        {
            if (_appSettings == null || _appSettings.Value == null)
                throw new InvalidOperationException("AppSettings is null");

            if ( string.IsNullOrEmpty(_appSettings.Value.SmtpServer) ||
                 string.IsNullOrEmpty(_appSettings.Value.SmtpPort) ||
                 string.IsNullOrEmpty(_appSettings.Value.SmtpUser) ||
                 string.IsNullOrEmpty(_appSettings.Value.SmtpPassword) ||
                 string.IsNullOrEmpty(_appSettings.Value.SmtpFromAddress))
                throw new InvalidOperationException("AppSettings is null or has a null Smtp value");

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
            _logger.LogError(ex.Message);
        }


    }

}

