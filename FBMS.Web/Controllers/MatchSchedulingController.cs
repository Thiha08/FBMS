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
    }
}
