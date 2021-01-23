using FBMS.Core.Ctos.Filters;
using FBMS.Core.Dtos.Filters;
using FBMS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FBMS.Web.Controllers
{
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;
        private readonly IMemberService _memberService;

        public TransactionController(ITransactionService transactionService, IMemberService memberService)
        {
            _transactionService = transactionService;
            _memberService = memberService;
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
    }
}
