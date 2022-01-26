using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server_api.Data.Models.Interfaces;
using Microsoft.AspNetCore.Http;

namespace server_api.Controllers
{
    [Authorize(Roles = "admins")]
    public class GenreController: Controller
    {
        private readonly IGenreRepository genreRepository;
        public GenreController(IGenreRepository genreRepository)
        {
            this.genreRepository = genreRepository;
        }

        public IActionResult Index()=>View();
    }
}
