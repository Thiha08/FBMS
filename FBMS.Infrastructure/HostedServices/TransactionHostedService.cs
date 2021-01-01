using FBMS.Core.Constants.Crawler;
using FBMS.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FBMS.Infrastructure.HostedServices
{
    public class TransactionHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<TransactionHostedService> _logger;
        private readonly ITransactionService _transactionService;
        private readonly IHostApiCrawlerSettings _hostApiCrawlerSettings;

        private int executionCount = 0;
        private Timer _timer;

        public TransactionHostedService(ILogger<TransactionHostedService> logger, ITransactionService transactionService, IHostApiCrawlerSettings hostApiCrawlerSettings)
        {
            _logger = logger;
            _transactionService = transactionService;
            _hostApiCrawlerSettings = hostApiCrawlerSettings;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Transaction Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMilliseconds(_hostApiCrawlerSettings.TriggerPerMilliseconds));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            try
            {
                var count = Interlocked.Increment(ref executionCount);

                _logger.LogInformation(
                    "Transaction Hosted Service is working. Count: {Count}", count);

                _transactionService.CrawlAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Transaction Hosted Service is stopped unexpectedly");
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Transaction Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
