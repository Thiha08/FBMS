using Ardalis.GuardClauses;
using FBMS.Core.Events;
using FBMS.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
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

            await _emailSender.SendEmailAsync("thihakyawhtin.mm@gmail.com", "thihakyawhtin.mm@gmail.com", $"{domainEvent.CompletedItem.Title} was completed.", domainEvent.CompletedItem.ToString());
        }
    }
}
