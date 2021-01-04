using FBMS.Core.Dtos.Filters;
using FBMS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FBMS.Web.Controllers
{
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<IActionResult> Index([FromQuery] TransactionFilterDto filter)
        {
            filter = filter ?? new TransactionFilterDto();

            filter.IsPagingEnabled = false;

            return View(await _transactionService.GetTransactions(filter));
        }

        public async Task<IActionResult> Crawl()
        {
            await _transactionService.CrawlTransactions();
            return RedirectToAction(nameof(Index));
        }
    }
}
