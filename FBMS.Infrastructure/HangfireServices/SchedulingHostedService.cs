using FBMS.Core.Constants;
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
                MaxDischargedCount = CoreConstants.MaxDischargedCount
            };

            var activeTransactions = await _transactionService.GetTransactions(filter);
            if (activeTransactions.Count == 0) return;

            activeTransactions = activeTransactions.OrderByDescending(x => x.DischargedCount)
                                                   .ThenByDescending(x => x.TransactionDate)
                                                   .ToList();

            var matchSchedule = await _matchSchedulingService.GetMatchSchedule();

            foreach (var transaction in activeTransactions)
            {
                try
                {
                    var selectedMatches = matchSchedule.Where(x =>
                        (
                            x.HomeTeam.TrimAndUpper() == transaction.HomeTeam.TrimAndUpper() ||
                            x.HomeTeam.TrimAndUpper() == transaction.HomeTeam.ConcatSuffix("(n)") ||
                            x.HomeTeam.TrimAndUpper() == transaction.HomeTeam.ConcatSuffix("(R)") ||
                            x.HomeTeam.TrimAndUpper() == transaction.HomeTeam.ConcatSuffix("(V)") ||
                            x.HomeTeam.TrimAndUpper() == transaction.HomeTeam.ConcatSuffix("(Youth) (n)")
                        )
                        &&
                        (
                            x.AwayTeam.TrimAndUpper() == transaction.AwayTeam.TrimAndUpper() ||
                            x.AwayTeam.TrimAndUpper() == transaction.AwayTeam.ConcatSuffix("(n)") ||
                            x.AwayTeam.TrimAndUpper() == transaction.AwayTeam.ConcatSuffix("(R)") ||
                            x.AwayTeam.TrimAndUpper() == transaction.AwayTeam.ConcatSuffix("(V)") ||
                            x.AwayTeam.TrimAndUpper() == transaction.AwayTeam.ConcatSuffix("(Youth) (n)")
                        )
                    ).ToList();

                    var matchUrl = await _matchSchedulingService.GetMatchTransactionUrl(transaction.SubmittedTransactionType, transaction.Pricing.ToAbsPricing(), selectedMatches);

                    if (string.IsNullOrWhiteSpace(matchUrl))
                    {
                        throw new Exception(TransactionResponseStatus.MatchNotFound);
                    }

                    _logger.LogWarning(
                        "Match Detail URL!" + Environment.NewLine +
                        matchUrl);

                    var matchDetail = await _matchSchedulingService.GetMatchDetail(matchUrl);

                    var matchBet = new MatchBetDto
                    {
                        BetUrl = matchDetail.BetUrl,
                        Stack = (int)transaction.SubmittedAmount
                    };

                    _logger.LogWarning(
                        "Match Detail:" + Environment.NewLine +
                        transaction.GetInfo());

                    var roundValue = Math.Round(transaction.SubmittedAmount, 0, MidpointRounding.AwayFromZero); // 0 for now
                    matchBet.Stack = Convert.ToInt32(roundValue);
                    var response = await _matchSchedulingService.SubmitMatchTransaction(matchBet);

                    _logger.LogWarning(response);

                    if (response.Contains(TransactionResponseStatus.OddChanged, StringComparison.CurrentCultureIgnoreCase))
                    {
                        throw new Exception(TransactionResponseStatus.OddChanged);
                    }
                    else if(response.Contains(TransactionResponseStatus.OddUnavailable, StringComparison.CurrentCultureIgnoreCase))
                    {
                        throw new Exception(TransactionResponseStatus.OddUnavailable);
                    }
                    else if(response.Contains(TransactionResponseStatus.StatusAccepted, StringComparison.CurrentCultureIgnoreCase))
                    {
                        await _transactionService.CompleteTransaction(transaction.Id, matchDetail.BetHdp, response);
                    }
                    else if (response.Contains(TransactionResponseStatus.StatusAccepted, StringComparison.CurrentCultureIgnoreCase))
                    {
                        await _transactionService.CompleteTransaction(transaction.Id, matchDetail.BetHdp, TransactionResponseStatus.OddUnavailable);
                    }
                    else
                    {
                        throw new Exception(response);
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogWarning(
                        "Discharge Transaction" + Environment.NewLine +
                        transaction.GetInfo() + Environment.NewLine +
                        exception.Message);

                    await _transactionService.DischargeTransaction(transaction.Id, exception.Message);
                }
            }
        }
    }
}
