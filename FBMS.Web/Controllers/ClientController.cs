using FBMS.Core.Dtos;
using FBMS.Core.Dtos.Filters;
using FBMS.Core.Interfaces;
using FBMS.Web.ApiModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBMS.Web.Controllers
{
    public class ClientController : BaseController
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<IActionResult> Index([FromQuery] ClientFilterDto filter)
        {
            filter = filter ?? new ClientFilterDto();

            filter.IsPagingEnabled = true;

            return View(await _clientService.GetClients(filter));
        }

        // GET: Worker/Create
        public async Task<IActionResult> Crawl()
        {
            await _clientService.CrawlClients();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enable(BaseDto dto)
        {
            await _clientService.EnableClient(dto.Id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable(BaseDto dto)
        {
            await _clientService.DisableClient(dto.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
