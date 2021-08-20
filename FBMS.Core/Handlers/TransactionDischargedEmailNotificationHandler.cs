using Ardalis.GuardClauses;
using FBMS.Core.Constants.Crawler;
using FBMS.Core.Constants.Email;
using FBMS.Core.Events;
using FBMS.Core.Extensions;
using FBMS.Core.Mail;
using MediatR;
using MimeKit;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IClientApiCrawlerSettings _clientApiCrawlerSettings;

        public TransactionDischargedEmailNotificationHandler(IEmailSender emailSender, IEmailSettings emailSettings, IEmailTemplateProvider emailTemplateProvider, IClientApiCrawlerSettings clientApiCrawlerSettings)
        {
            _emailSender = emailSender;
            _emailSettings = emailSettings;
            _emailTemplateProvider = emailTemplateProvider;
            _clientApiCrawlerSettings = clientApiCrawlerSettings;
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
            emailTemplate.Replace("{{TRANSACTION_DATE}}", transaction.TransactionDate.ToTimeZoneTimeString("yyyy-MM-dd HH:mm:ss"));
            emailTemplate.Replace("{{DISCHARGED_DATE}}", transaction.DischargedDate?.ToTimeZoneTimeString("yyyy-MM-dd HH:mm:ss"));
            emailTemplate.Replace("{{HOME_TEAM}}", transaction.HomeTeam);
            emailTemplate.Replace("{{AWAY_TEAM}}", transaction.AwayTeam);
            emailTemplate.Replace("{{PRICING}}", transaction.Pricing);
            emailTemplate.Replace("{{DISCHARGED_PRICING}}", "");
            emailTemplate.Replace("{{TYPE}}", transaction.TransactionType.ToString());
            emailTemplate.Replace("{{DISCHARGED_TYPE}}", transaction.SubmittedTransactionType.ToString());
            emailTemplate.Replace("{{AMOUNT}}", transaction.Amount.ToString());
            emailTemplate.Replace("{{DISCHARGED_AMOUNT}}", $"{transaction.SubmittedAmount}{(_clientApiCrawlerSettings.IsTestingStack ? " (Testing Amount: 1)" : "")}");
            emailTemplate.Replace("{{MESSAGE}}", domainEvent.Message);

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_emailSettings.SenderEmail));
            recipients.ForEach(recipient => message.To.Add(MailboxAddress.Parse(recipient)));
            message.Subject = $"{ transaction.TransactionNumber } was discharged.";
            message.Body = new TextPart("html")
            {
                Text = emailTemplate.ToString()
            };

            _ = Task.Run(() => _emailSender.SendAsync(message).ConfigureAwait(false));
        }
    }
}
