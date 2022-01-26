using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server_api.Data.Models;
using server_api.Data.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server_api.Controllers
{
    [Authorize(Roles = "admins")]
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<User> userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var result = (Repository.getRolesWithUsersAsync(roleManager.Roles.ToArray(), userManager)).ToList();
            return View(result);
        }

        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (ModelState.IsValid)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                };
                AddIdentityError(result);
            }
            return View(name);

        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                AddIdentityError(result);
            }
            else
            {
                ModelState.AddModelError("", "No role found");
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ModelState.AddModelError("", "No role found");
            }
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string name)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ModelState.AddModelError("", "No role found");
            }
            else if (role.Name == name || string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("", "Role was not updated");
            }
            else
            {
                role.Name = name;
                var result = await roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    ModelState.AddErrors(string.Empty, result.Errors.Select(err => err.Description));
                }
            }
            return RedirectToAction(nameof(Index));
        }
        public void AddIdentityError(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            };
        }

    }
}
