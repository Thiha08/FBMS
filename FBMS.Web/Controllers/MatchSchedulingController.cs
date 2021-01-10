using FBMS.Core.Dtos;
using FBMS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBMS.Web.Controllers
{
    public class MatchSchedulingController : BaseController
    {
        private readonly IMatchSchedulingService _matchSchedulingService;

        public MatchSchedulingController(IMatchSchedulingService matchSchedulingService)
        {
            _matchSchedulingService = matchSchedulingService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _matchSchedulingService.GetMatchSchedule());
        }

        public async Task<IActionResult> Edit(string matchUrl)
        {
            return View(await _matchSchedulingService.GetMatchDetail(matchUrl));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MatchBetDto input)
        {
            var response = await _matchSchedulingService.SubmitMatchTransaction(input);
            GenerateAlertMessage(true, response);
            return RedirectToAction(nameof(Index));
        }
    }
}
