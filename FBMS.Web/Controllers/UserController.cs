using FBMS.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBMS.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var viewModels = new List<UserDto>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                viewModels.Add(new UserDto()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Role = userRoles.FirstOrDefault()
                });
            }

            return View(viewModels);
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();

            return View();
        }

        // POST: Worker/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserDto viewModel)
        {
            try
            {
                ViewBag.Roles = await _roleManager.Roles.ToListAsync();

                // TODO: Add insert logic here
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                var model = new IdentityUser
                {
                    UserName = viewModel.UserName,
                    Email = viewModel.Email,
                    PhoneNumber = viewModel.PhoneNumber
                };

                var result = await _userManager.CreateAsync(model, viewModel.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    //GenerateAlertMessage(false, "The worker is failed to create.");

                    return View(viewModel);
                }

                var user = await _userManager.FindByNameAsync(model.UserName);

                var role = await _roleManager.FindByIdAsync(viewModel.Role);

                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }

                //GenerateAlertMessage(true, "The worker is created successfully.");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                //GenerateAlertMessage(false, "The worker is failed to create.");

                return View();
            }
        }

        // GET: Worker/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = await _roleManager.Roles.ToListAsync();

            var userRoles = await _userManager.GetRolesAsync(user);

            var viewModel = new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Role = userRoles.FirstOrDefault()
            };

            return View(viewModel);
        }

        // POST: Worker/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserDto viewModel)
        {
            try
            {
                ViewBag.Roles = await _roleManager.Roles.ToListAsync();

                // TODO: Add insert logic here
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                var model = await _userManager.FindByIdAsync(viewModel.Id);

                if (model == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                //model.UserName = viewModel.Email;
                model.Email = viewModel.Email;
                model.PhoneNumber = viewModel.PhoneNumber;
              
                var result = await _userManager.UpdateAsync(model);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    //GenerateAlertMessage(false, "The worker is failed to update.");

                    return View(viewModel);
                }

                var userRoles = await _userManager.GetRolesAsync(model);

                if (userRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(model, userRoles.ToArray());
                }

                var role = await _roleManager.FindByIdAsync(viewModel.Role);

                if (role != null)
                {
                    await _userManager.AddToRoleAsync(model, role.Name);
                }

                //GenerateAlertMessage(true, "The worker is updated successfully.");

                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View();
            }
        }

        // POST: Worker/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
