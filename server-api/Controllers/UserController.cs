using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server_api.Data.Models;
using server_api.Data.Models.Repositories;
using server_api.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server_api.Controllers
{
    [Authorize(Roles = "admins")]
    public class UserController : Controller
    {
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;
        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<ViewResult> Index()
        {
            var users = userManager.Users.OrderBy(user=>user.Id).ToList();
            var result = new List<Tuple<User, IList<string>>>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                result.Add(Tuple.Create(user, roles));
            }
            return View(result);
        }
        public ViewResult Create(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    UserName = model.Name,
                    Email = model.Email
                };

                IdentityResult result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await userManager.setUserRolesAsync(user, model.Roles);
                    return RedirectToAction(nameof(Index));
                };
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, $"User by id = {id} can not be delete - user not found");
                return RedirectToAction("Index");
            }
            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddErrors(string.Empty, result.Errors.Select(it => it.Description).ToArray());
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var userRoles = await userManager.GetRolesAsync(user);
            var allRoles = roleManager.Roles.ToList();



            if (user != null)
            {
                var roles = allRoles.Select(role => Tuple.Create(
                role.Name,
                userRoles.Contains(role.Name)
            )).ToList();
                ViewBag.CheckBoxData = roles.Select(pair =>
                {
                    return new CheckBoxListViewModel
                    {
                        Name = "Roles",
                        Value = pair.Item1,
                        Enabled = true,
                        Caption = pair.Item1,
                        Checked = pair.Item2,
                        Visible = true,
                    };
                }).ToList();
                return View(new UserViewModel { Id = user.Id, Name = user.UserName, Email = user.Email, Roles = userRoles });
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            var existUser = await userManager.FindByIdAsync(user.Id);
            if (existUser == null)
            {
                ModelState.AddModelError("", $"Не найден пользователь с id = {user.Id}");
                return RedirectToAction(nameof(Create), new UserRegisterViewModel { Name = user.Name, Email = user.Email });
            }
            existUser.UserName = user.Name;
            existUser.Email = user.Email;
            var result = await userManager.UpdateAsync(existUser);
            if (result.Succeeded)
            {

                await userManager.setUserRolesAsync(existUser, user.Roles);
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddErrors(string.Empty, result.Errors.Select(it => it.Description).ToArray());
            return View(user);
        }
    }
}
