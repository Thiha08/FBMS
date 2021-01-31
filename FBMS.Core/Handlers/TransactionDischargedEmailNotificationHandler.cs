using Ardalis.GuardClauses;
using FBMS.Core.Constants.Email;
using FBMS.Core.Events;
using FBMS.Core.Extensions;
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
    public class TransactionDischargedEmailNotificationHandler : INotificationHandler<TransactionDischargedEvent>
    {
        private readonly IEmailSender _emailSender;
        private readonly IEmailSettings _emailSettings;
        private readonly IEmailTemplateProvider _emailTemplateProvider;

        public TransactionDischargedEmailNotificationHandler(IEmailSender emailSender, IEmailSettings emailSettings, IEmailTemplateProvider emailTemplateProvider)
        {
            _emailSender = emailSender;
            _emailSettings = emailSettings;
            _emailTemplateProvider = emailTemplateProvider;
        }

        public async Task Handle(TransactionDischargedEvent domainEvent, CancellationToken cancellationToken)
        {
            Guard.Against.Null(domainEvent, nameof(domainEvent));

            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTransactionDischargedEmailTemplate());
            List<string> recipients = _emailSettings.Recipients.Split(',').ToList<string>();
            var transaction = domainEvent.DischargedTransaction;

            emailTemplate.Replace("{{TRANSACTION_NUMBER}}", transaction.TransactionNumber);
            emailTemplate.Replace("{{LEAGUE}}", transaction.League);
            emailTemplate.Replace("{{ACCOUNT}}", transaction.UserName);
            emailTemplate.Replace("{{TRANSACTION_DATE}}", transaction.TransactionDate.ToTimeZoneTime("dd-MM-yyyy HH:mm:ss"));
            emailTemplate.Replace("{{HOME_TEAM}}", transaction.HomeTeam);
            emailTemplate.Replace("{{AWAY_TEAM}}", transaction.AwayTeam);
            emailTemplate.Replace("{{PRICING}}", transaction.Pricing);
            emailTemplate.Replace("{{TYPE}}", transaction.TransactionType.ToString());
            emailTemplate.Replace("{{AMOUNT}}", transaction.Amount.ToString());
            emailTemplate.Replace("{{DISCHARGED_DATE}}", transaction.DischargedDate?.ToTimeZoneTime("dd-MM-yyyy HH:mm:ss"));
            emailTemplate.Replace("{{MESSAGE}}", domainEvent.Message);

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_emailSettings.SenderEmail));
            recipients.ForEach(recipient => message.To.Add(MailboxAddress.Parse(recipient)));
            message.Subject = $"{ transaction.TransactionNumber } was discharged.";
            message.Body = new TextPart("html")
            {
                Text = emailTemplate.ToString()
            };
            await _emailSender.SendAsync(message);
        }
    }
}
