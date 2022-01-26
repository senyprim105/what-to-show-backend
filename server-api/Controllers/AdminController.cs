using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace server_api.Controllers
{
    [Authorize(Roles = "admins")]
    public class AdminController : Controller
    {
        public IActionResult Index() => View();
    }
}
