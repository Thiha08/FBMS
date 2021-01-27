using Ardalis.GuardClauses;
using FBMS.Core.Events;
using FBMS.Core.Interfaces;
using FBMS.Core.Mail;
using MediatR;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBMS.Core.Handlers
{
    public class ItemCompletedEmailNotificationHandler : INotificationHandler<ToDoItemCompletedEvent>
    {
        private readonly IEmailSender _emailSender;

        public ItemCompletedEmailNotificationHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        // configure a test email server to demo this works
        // https://ardalis.com/configuring-a-local-test-email-server
        public async Task Handle(ToDoItemCompletedEvent domainEvent, CancellationToken cancellationToken)
        {
            Guard.Against.Null(domainEvent, nameof(domainEvent));
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse("TEST"));
            message.To.Add(MailboxAddress.Parse("thihakyawhtin.mm@gmail.com"));
            message.Subject = $"{domainEvent.CompletedItem.Title} was completed.";
            message.Body = new TextPart("plain")
            {
                Text = domainEvent.CompletedItem.ToString()
            };

            await _emailSender.SendAsync(message);
        }
    }
}
