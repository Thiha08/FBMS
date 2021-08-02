using FBMS.Core.Dtos;
using FBMS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FBMS.Web.Controllers
{
    public class SettingController : BaseController
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _settingService.GetSettings());
        }

        public async Task<IActionResult> Enable(int id)
        {
            await _settingService.EnableSetting(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Disable(int id)
        {
            await _settingService.DisableSetting(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _settingService.DeleteSetting(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Create()
        {
            var dto = new SettingDto();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SettingDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                await _settingService.CreateSetting(dto);
                GenerateAlertMessage(true, "The setting is created successfully.");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                GenerateAlertMessage(false, "The setting is failed to create.");
                return View();
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            return View(await _settingService.GetSetting(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SettingDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                await _settingService.UpdateSetting(dto);
                GenerateAlertMessage(true, "The setting is updated successfully.");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                GenerateAlertMessage(false, "The setting is failed to update.");
                return View();
            }
        }
    }
}
