using Ardalis.GuardClauses;
using FBMS.Core.Events;
using FBMS.Core.Specifications;
using FBMS.SharedKernel.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBMS.Core.Handlers
{
    public class TransactionTemplateAddedEventNotificationHandler : INotificationHandler<TransactionTemplateAddedEvent>
    {
        private readonly IRepository _repository;

        public TransactionTemplateAddedEventNotificationHandler(IRepository repository)
        {
            _repository = repository;
        }

        // configure a test email server to demo this works
        // https://ardalis.com/configuring-a-local-test-email-server
        public async Task Handle(TransactionTemplateAddedEvent domainEvent, CancellationToken cancellationToken)
        {
            Guard.Against.Null(domainEvent, nameof(domainEvent));
            var template = domainEvent.Template;
            foreach (var item in template.TemplateItems)
            {
                item.Status = item.TransactionType != Constants.TransactionType.Parlay;
            }
            await _repository.UpdateAsync(template);
        }
    }
}
