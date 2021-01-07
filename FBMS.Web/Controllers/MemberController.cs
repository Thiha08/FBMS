using FBMS.Core.Dtos;
using FBMS.Core.Dtos.Filters;
using FBMS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FBMS.Web.Controllers
{
    public class MemberController : BaseController
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public async Task<IActionResult> Index([FromQuery] MemberFilterDto filter)
        {
            filter = filter ?? new MemberFilterDto();

            filter.IsPagingEnabled = false;

            return View(await _memberService.GetMembers(filter));
        }

        public async Task<IActionResult> Crawl()
        {
            await _memberService.CrawlMembers();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Enable(int id)
        {
            await _memberService.EnableMember(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Disable(int id)
        {
            await _memberService.DeleteMember(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> EditTransactionTemplate(int id)
        {
            return View(await _memberService.GetMemberWithTransactionTemplate(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTransactionTemplate(MemberTransactionTemplateDto input)
        {
            await _memberService.UpdateMemberWithTransactionTemplate(input);
            GenerateAlertMessage(true, "The template is updated successfully.");
            return RedirectToAction(nameof(EditTransactionTemplate), input.MemberId);
        }
    }
}
