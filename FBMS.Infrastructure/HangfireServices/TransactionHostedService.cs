using FBMS.Core.Constants.Hangfire;
using FBMS.Core.Ctos.Filters;
using FBMS.Core.Interfaces;
using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBMS.Infrastructure.HangfireServices
{
    public class TransactionHostedService : ITransactionHostedService
    {
        private readonly ILogger<TransactionHostedService> _logger;
        private readonly IHangfireSettings _hangfireSettings;
        private readonly ITransactionService _transactionService;
        private readonly IMemberService _memberService;

        public TransactionHostedService(ILogger<TransactionHostedService> logger, IHangfireSettings hangfireSettings, ITransactionService transactionService, IMemberService memberService)
        {
            _logger = logger;
            _hangfireSettings = hangfireSettings;
            _transactionService = transactionService;
            _memberService = memberService;
        }

        public Task StartAsync()
        {
            _logger.LogWarning("Transaction Hosted Service running.");

            RecurringJob.AddOrUpdate<ITransactionHostedService>(_hangfireSettings.TransactionJobIdentifier, x => x.RecurringTransactionJob(), _hangfireSettings.CronExpression);

            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            _logger.LogWarning("Transaction Hosted Service is stopping.");

            RecurringJob.RemoveIfExists(_hangfireSettings.TransactionJobIdentifier);

            return Task.CompletedTask;
        }

        public Task<bool> IsRunning()
        {
            List<RecurringJobDto> recurringJobs = new List<RecurringJobDto>();
            recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs().ToList();

            bool isRunning = recurringJobs.Any(x => x.Id == _hangfireSettings.TransactionJobIdentifier);
            return Task.FromResult(isRunning);
        }

        public async Task RecurringTransactionJob()
        {
            var filter = new TransactionFilterCto();
            filter.MemberIds = await _memberService.CrawlActiveMembers();
            filter.StartDate = DateTime.UtcNow;
            filter.EndDate = DateTime.UtcNow;
            await _transactionService.CrawlTransactions(filter);
        }
    }
}
