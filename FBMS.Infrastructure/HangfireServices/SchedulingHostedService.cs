using FBMS.Core.Constants;
using FBMS.Core.Constants.Hangfire;
using FBMS.Core.Ctos.Filters;
using FBMS.Core.Dtos;
using FBMS.Core.Dtos.Filters;
using FBMS.Core.Extensions;
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
            var filter = new TransactionFilterDto();
            filter.IsSubmitted = false;
            filter.IsPagingEnabled = false;
            var activeTransactions = await _transactionService.GetTransactions(filter);

            var matchSchedule = await _matchSchedulingService.GetMatchSchedule();
            foreach (var transaction in activeTransactions)
            {
                var selectedMatchs = matchSchedule
                    .Where(x => x.HomeTeam == transaction.HomeTeam)
                    .Where(x => x.AwayTeam == transaction.AwayTeam);

                var matchUrl = "";

                if (transaction.SubmittedTransactionType == TransactionType.Home)
                {
                    var fixedMatch = selectedMatchs.Closest(x => x.FtHdpPricing, Convert.ToDecimal(transaction.Pricing));
                    matchUrl = fixedMatch?.GetHomeUrl();
                }
                else if(transaction.SubmittedTransactionType == TransactionType.Away)
                {
                    var fixedMatch = selectedMatchs.Closest(x => x.FtHdpPricing, Convert.ToDecimal(transaction.Pricing));
                    matchUrl = fixedMatch?.GetAwayUrl();
                }
                else if(transaction.SubmittedTransactionType == TransactionType.Over)
                {
                    var fixedMatch = selectedMatchs.Closest(x => x.FtOuPricing, Convert.ToDecimal(transaction.Pricing));
                    matchUrl = fixedMatch?.GetOverUrl();
                }
                else if(transaction.SubmittedTransactionType == TransactionType.Under)
                {
                    var fixedMatch = selectedMatchs.Closest(x => x.FtOuPricing, Convert.ToDecimal(transaction.Pricing));
                    matchUrl = fixedMatch?.GetUnderUrl();
                }

                if (string.IsNullOrWhiteSpace(matchUrl))
                {
                   
                }
                else
                {
                    var matchDetail = await _matchSchedulingService.GetMatchDetail(matchUrl);

                    var matchBet = new MatchBetDto();
                    matchBet.BetUrl = matchDetail.BetUrl;
                    matchBet.Stack = (int)transaction.SubmittedAmount;
                    matchBet.Stack = 0; // 0 for now;
                    var response = await _matchSchedulingService.SubmitMatchTransaction(matchBet);
                    if (response.Contains("SUCCESSFULLY"))
                    {
                        transaction.SubmittedDate = DateTime.Now;
                        transaction.SubmittedPricing = matchDetail.BetHdp;
                        transaction.IsSubmitted = true;
                        // break for loop
                        break;
                    }
                }
            }
        }
    }
}
