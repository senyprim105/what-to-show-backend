using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server_api.Data.Models.Interfaces;
using Microsoft.AspNetCore.Http;

namespace server_api.Controllers
{
    [Authorize(Roles = "admins")]
    public class PersonController: Controller
    {
        private readonly IPersonRepository personRepository;
        public PersonController(IPersonRepository personalRepository)
        {
            this.personRepository = personalRepository;
        }

        public IActionResult Index(){
            return View("gijgo");
        }
    }
}
