using FBMS.Core.Constants.Email;
using FBMS.Core.Mail;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace FBMS.Infrastructure.Mail
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly IEmailSettings _emailSettings;

        public EmailSender(ILogger<EmailSender> logger, IEmailSettings emailSettings)
        {
            _logger = logger;
            _emailSettings = emailSettings;
        }

        public async Task SendAsync(MimeMessage mail)
        {
            _logger.LogInformation("Email sending started");
            try
            {
                using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
                await client.SendAsync(mail);
                await client.DisconnectAsync(true);
                _logger.LogInformation($"Email sent to {mail.To}");
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Couldn't send an email to {recipient}", mail.To[0]);
            }
        }
    }
}
