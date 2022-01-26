using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server_api.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server_api.Controllers
{
    public class HomeController:Controller
    {
        [TempDataToModelStateErrors]
        public ViewResult About() => View();
        public ViewResult AccessDenided()=>View();
        [Authorize]
        public ViewResult Index() => View();

    }
}
