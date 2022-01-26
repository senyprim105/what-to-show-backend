using System.Threading;
using System.Runtime.Serialization.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using server_api.Data.Models;
using server_api.Data.ViewModels;
using server_api.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server_api.Auth;
using server_api.Data.Models.Repositories;
using Newtonsoft.Json;

namespace server_api.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signIn;
        private string loginPath = null;
        private IHttpContextAccessor httpContext;
        //private IJwtFactory _jwtFactory;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signIn,
            IOptionsMonitor<CookieAuthenticationOptions> monitor,
            IHttpContextAccessor httpContextAccessor
           // IJwtFactory jwtFactory
           )
        {
            this.userManager = userManager;
            this.signIn = signIn;
            this.loginPath = monitor.Get(IdentityConstants.ApplicationScheme).LoginPath;
            this.httpContext = httpContextAccessor;
           // this._jwtFactory = jwtFactory;
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return RedirectToAction("About", "Home");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ModelStateErrorsToTempData]
        public async Task<IActionResult> Login(UserLoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return LocalRedirect(loginModel.ReturnUrl ?? loginPath);
            }
            var user = await userManager.FindByNameAsync(loginModel.Name);
            if (user == null)
            {
                ModelState.AddModelError("", "Пользователя с таким именем не существует");
                return LocalRedirect(loginModel.ReturnUrl ?? loginPath);
            }
            await signIn.SignOutAsync();
            var result = await signIn.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Неправильный логин и(или) пароль");
                return LocalRedirect(loginModel.ReturnUrl ?? loginPath);
            }
            //Сохраняем в сессию jwttoken залогиненного 
            // System.Diagnostics.Debug.WriteLine(HttpContext.Session.GetString("token"));
            // var jwtToken = await _jwtFactory.GenerateJwt(
            //     await Repository.GetClaimsIdentity(loginModel.Name, loginModel.Password, userManager)
            //     , loginModel.Name);

            // if (HttpContext.Session.Keys.Contains("token"))
            //     HttpContext.Session.Remove("token");
            // HttpContext.Session.SetString("token", jwtToken);


            return LocalRedirect(loginModel.ReturnUrl ?? loginPath);
        }
        public async Task<IActionResult> LogOut()
        {
            await signIn.SignOutAsync();
            return LocalRedirect(loginPath);
        }

        public IActionResult AccessDenied()=>View();
    }
}
