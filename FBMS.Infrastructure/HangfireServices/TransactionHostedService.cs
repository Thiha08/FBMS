using FBMS.Core.Constants.Hangfire;
using FBMS.Core.Interfaces;
using Hangfire;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FBMS.Infrastructure.HangfireServices
{
    public class TransactionHostedService : ITransactionHostedService
    {
        private readonly ILogger<TransactionHostedService> _logger;
        private readonly IHangfireSettings _hangfireSettings;

        public TransactionHostedService(ILogger<TransactionHostedService> logger, IHangfireSettings hangfireSettings)
        {
            _logger = logger;
            _hangfireSettings = hangfireSettings;
        }

        public Task StartAsync()
        {
            _logger.LogInformation("Transaction Hosted Service running.");

            RecurringJob.AddOrUpdate<ITransactionService>(_hangfireSettings.RecurringJobIdentifier, x => x.CrawlTransactions(), _hangfireSettings.CronExpression);

            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            _logger.LogInformation("Transaction Hosted Service is stopping.");

            RecurringJob.RemoveIfExists(_hangfireSettings.RecurringJobIdentifier);

            return Task.CompletedTask;
        }
    }
}
