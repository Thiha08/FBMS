using FBMS.Core.Ctos.Filters;
using FBMS.Core.Dtos;
using FBMS.Core.Dtos.Filters;
using FBMS.Core.Extensions;
using FBMS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FBMS.Web.Controllers
{
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;
        private readonly IMemberService _memberService;
        private readonly ILogger<TransactionController> _logger;
        private readonly IMatchSchedulingService _matchSchedulingService;

        public TransactionController(ITransactionService transactionService, IMemberService memberService, ILogger<TransactionController> logger, IMatchSchedulingService matchSchedulingService)
        {
            _transactionService = transactionService;
            _memberService = memberService;
            _logger = logger;
            _matchSchedulingService = matchSchedulingService;
        }

        public async Task<IActionResult> Index([FromQuery] TransactionFilterDto filter)
        {
            filter ??= new TransactionFilterDto();
            return View(await _transactionService.GetTransactions(filter));
        }

        public async Task<IActionResult> Crawl()
        {
            var filter = new TransactionFilterCto();

            filter.MemberIds = await _memberService.CrawlActiveMembers();
            filter.StartDate = DateTime.UtcNow;
            filter.EndDate = DateTime.UtcNow;

            await _transactionService.CrawlTransactions(filter);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Submit()
        {
            var filter = new TransactionFilterDto
            {
                IsSubmitted = false,
                IsDischarged = false
            };

            var activeTransactions = await _transactionService.GetTransactions(filter);
            if (activeTransactions.Count == 0) return RedirectToAction(nameof(Index));

            var matchSchedule = await _matchSchedulingService.GetMatchSchedule();
            foreach (var transaction in activeTransactions)
            {
                var selectedMatches = matchSchedule.Where(x => x.HomeTeam == transaction.HomeTeam && x.AwayTeam == transaction.AwayTeam).ToList();

                var matchUrl = await _matchSchedulingService.GetMatchTransactionUrl(transaction.SubmittedTransactionType, transaction.Pricing.ToAbsPricing(), selectedMatches);

                if (string.IsNullOrWhiteSpace(matchUrl))
                {
                    _logger.LogError(
                        "Cannot find related Match Detail!" + Environment.NewLine +
                        transaction.GetInfo());
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
                        "*** Match Detail:" + Environment.NewLine +
                        transaction.GetInfo());

                    //matchBet.Stack = 0; // 0 for now;
                    //var response = await _matchSchedulingService.SubmitMatchTransaction(matchBet);
                    //if (response.Contains("SUCCESSFULLY"))
                    //{
                    //    transaction.SubmittedDate = DateTime.Now;
                    //    transaction.SubmittedPricing = matchDetail.BetHdp;
                    //    transaction.IsSubmitted = true;
                    //    await _transactionService.UpdateTransaction(transaction);
                    //}
                    //_logger.LogWarning(response);
                }

                // TO PREVENT LOGGING AGAIN AND AGAIN
                transaction.SubmittedDate = DateTime.UtcNow;
                transaction.SubmittedPricing = transaction.Pricing;
                transaction.IsSubmitted = true;

                await _transactionService.UpdateTransaction(transaction);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
