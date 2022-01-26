using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;
using server_api.Data.Models;
using server_api.Data.Models.Interfaces;
using server_api.Data.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace server_api.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository movieRepository;
        private readonly IFileRepository fileRepository;
        private readonly IPersonRepository personRepository;

        public MovieController(IMovieRepository movieRepository, IFileRepository fileRepository, IPersonRepository personRepository)
        {
            this.movieRepository = movieRepository;
            this.fileRepository = fileRepository;
            this.personRepository = personRepository;
        }

        public IActionResult Index()
        {
            var movies = movieRepository.ReadAll;
            return View(movies);
        }
        public IActionResult Create()
        {
            var ImageList = fileRepository.ReadAll.Where(file => file.Type == FileAppType.image)
                .Select(file => new SelectListItem
                {
                    Value = file.Id.ToString(),
                    Text = file.OldName
                }) ;
            var MovieList = fileRepository.ReadAll.Where(file => file.Type == FileAppType.video).Select(file => new SelectListItem
                {
                    Value = file.Id.ToString(),
                    Text = file.OldPath
                });
            var  
                PersonList= personRepository.ReadAll.Select(person => new SelectListItem
                {
                    Value = person.Id.ToString(),
                    Text = person.Name
                });
            var model = new MovieViewModel
            {
                Movie = new Movie(),
                ImageList = ImageList,
                MovieList= MovieList,
                PersonList = PersonList
            };
            return View(model);
        }
    }
}
