using FBMS.Core.Constants.Crawler;
using FBMS.Core.Dtos.Crawler;
using FBMS.Core.Entities;
using FBMS.Core.Interfaces;
using FBMS.SharedKernel.Interfaces;
using FBMS.Web.ApiModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FBMS.Web.Controllers
{
    public class IBetController : Controller
    {
        private readonly IRepository _repository;
        private readonly IClientService _clientService;
        private readonly ITransactionService _transactionService;

        public IBetController(IRepository repository, IClientService clientService, ITransactionService transactionService)
        {
            _repository = repository;
            _clientService = clientService;
            _transactionService = transactionService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> CrawlClients()
        {
            await _clientService.CrawlAsync();
            return RedirectToAction(nameof(Clients));
        }

        public async Task<IActionResult> Clients()
        {
            var items = (await _clientService.ListAsync())
                .Select(ClientDTO.FromClient);
            return View(items);
        }

        public async Task<IActionResult> DeleteClients()
        {
            await _clientService.DeleteAllAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Transactions(int clientId)
        {
            ViewBag.ClientId = clientId;

            var items = (await _transactionService.ListAsync(clientId))
                .Select(TransactionDTO.FromTransaction);
            return View(items);
        }

        public async Task<IActionResult> CrawlTransactions(int clientId)
        {
            await _transactionService.CrawlAsync(clientId);
            return RedirectToAction(nameof(Transactions), new { clientId });
        }

        public async Task<IActionResult> DeleteTransactions(int clientId)
        {
            await _transactionService.DeleteAllAsync(clientId);
            return RedirectToAction(nameof(Transactions), new { clientId });
        }

        public async Task<IActionResult> CrawlAllTransactions()
        {
            await _transactionService.CrawlAsync();
            return RedirectToAction(nameof(AllTransactions));
        }

        public async Task<IActionResult> AllTransactions()
        {
            var items = (await _transactionService.ListAsync())
                .Select(TransactionDTO.FromTransaction);
            return View(items);
        }

        public async Task<IActionResult> DeleteAllTransactions()
        {
            await _transactionService.DeleteAllAsync();
            return RedirectToAction(nameof(AllTransactions));
        }
    }
}
