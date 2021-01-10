using FBMS.Core.Dtos;
using FBMS.Core.Interfaces;
using Hangfire.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBMS.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ITransactionHostedService _transactionHostedService;
        private readonly ISchedulingHostedService _schedulingHostedService;

        public HomeController(ITransactionHostedService transactionHostedService, ISchedulingHostedService schedulingHostedService)
        {
            _transactionHostedService = transactionHostedService;
            _schedulingHostedService = schedulingHostedService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardDto();
            viewModel.IsRunningTransactionJob = await _transactionHostedService.IsRunning();
            viewModel.IsRunningSchedulingJob = await _schedulingHostedService.IsRunning();
            return View(viewModel);
        }

        public async Task<IActionResult> StartTransactionJob()
        {
            await _transactionHostedService.StartAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> StopTransactionJob()
        {
            await _transactionHostedService.StopAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> StartSchedulingJob()
        {
            await _schedulingHostedService.StartAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> StopSchedulingJob()
        {
            await _schedulingHostedService.StopAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
