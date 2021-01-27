using Ardalis.GuardClauses;
using FBMS.Core.Constants.Email;
using FBMS.Core.Events;
using FBMS.Core.Mail;
using MediatR;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBMS.Core.Handlers
{
    public class TransactionCompletedEmailNotificationHandler : INotificationHandler<TransactionCompletedEvent>
    {
        private readonly IEmailSender _emailSender;
        private readonly IEmailSettings _emailSettings;
        private readonly IEmailTemplateProvider _emailTemplateProvider;

        public TransactionCompletedEmailNotificationHandler(IEmailSender emailSender, IEmailSettings emailSettings, IEmailTemplateProvider emailTemplateProvider)
        {
            _emailSender = emailSender;
            _emailSettings = emailSettings;
            _emailTemplateProvider = emailTemplateProvider;
        }

        public async Task Handle(TransactionCompletedEvent domainEvent, CancellationToken cancellationToken)
        {
            Guard.Against.Null(domainEvent, nameof(domainEvent));

            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTransactionCompletedEmailTemplate());
            List<string> recipients = _emailSettings.Recipients.Split(',').ToList<string>();

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_emailSettings.SenderName));
            recipients.ForEach(recipient => message.To.Add(MailboxAddress.Parse(recipient)));
            message.Subject = $"{ domainEvent.CompletedTransaction.TransactionNumber } was completed.";
            message.Body = new TextPart("html")
            {
                Text = emailTemplate.ToString()
            };
            await _emailSender.SendAsync(message);
        }
    }
}
