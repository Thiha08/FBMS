using FBMS.Core.Constants.Hangfire;
using FBMS.Core.Dtos;
using FBMS.Core.Dtos.Filters;
using FBMS.Core.Extensions;
using FBMS.Core.Interfaces;
using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBMS.Infrastructure.HangfireServices
{
    public class SchedulingHostedService : ISchedulingHostedService
    {
        private readonly ILogger<SchedulingHostedService> _logger;
        private readonly IHangfireSettings _hangfireSettings;
        private readonly ITransactionService _transactionService;
        private readonly IMatchSchedulingService _matchSchedulingService;

        public SchedulingHostedService(ILogger<SchedulingHostedService> logger, IHangfireSettings hangfireSettings, ITransactionService transactionService, IMatchSchedulingService matchSchedulingService)
        {
            _logger = logger;
            _hangfireSettings = hangfireSettings;
            _transactionService = transactionService;
            _matchSchedulingService = matchSchedulingService;
        }

        public Task StartAsync()
        {
            _logger.LogWarning("Scheduling Hosted Service running.");

            RecurringJob.AddOrUpdate<ISchedulingHostedService>(_hangfireSettings.SchedulingJobIdentifier, x => x.RecurringTransactionJob(), _hangfireSettings.CronExpression);

            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            _logger.LogWarning("Scheduling Hosted Service is stopping.");

            RecurringJob.RemoveIfExists(_hangfireSettings.SchedulingJobIdentifier);

            return Task.CompletedTask;
        }

        public Task<bool> IsRunning()
        {
            List<RecurringJobDto> recurringJobs = new List<RecurringJobDto>();
            recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs().ToList();

            bool isRunning = recurringJobs.Any(x => x.Id == _hangfireSettings.SchedulingJobIdentifier);
            return Task.FromResult(isRunning);
        }

        public async Task RecurringTransactionJob()
        {
            var filter = new TransactionFilterDto
            {
                IsSubmitted = false,
                IsDischarged = false
            };

            var activeTransactions = await _transactionService.GetTransactions(filter);
            if (activeTransactions.Count == 0) return;

            var matchSchedule = await _matchSchedulingService.GetMatchSchedule();
            foreach (var transaction in activeTransactions)
            {
                var selectedMatches = matchSchedule.Where(x => x.HomeTeam == transaction.HomeTeam && x.AwayTeam == transaction.AwayTeam).ToList();

                var matchUrl = await _matchSchedulingService.GetMatchTransactionUrl(transaction.SubmittedTransactionType, transaction.Pricing.ToAbsPricing(), selectedMatches);

                if (string.IsNullOrWhiteSpace(matchUrl))
                {
                    _logger.LogWarning(
                        "Cannot find related Match Detail!" + Environment.NewLine +
                        transaction.GetInfo());

                    await _transactionService.DischargeTransaction(transaction.Id);
                }
                else
                {
                    var matchDetail = await _matchSchedulingService.GetMatchDetail(matchUrl);

                    var matchBet = new MatchBetDto
                    {
                        BetUrl = matchDetail.BetUrl,
                        Stack = (int)transaction.SubmittedAmount
                    };

                    _logger.LogWarning(
                        "Match Detail:" + Environment.NewLine +
                        transaction.GetInfo());

                    matchBet.Stack = 0; // 0 for now
                    var response = await _matchSchedulingService.SubmitMatchTransaction(matchBet);

                    _logger.LogWarning(response);
                    await _transactionService.CompleteTransaction(transaction.Id, matchDetail.BetHdp, response);
                }
            }
        }
    }
}
