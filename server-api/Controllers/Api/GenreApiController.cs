using Microsoft.VisualBasic;
using System.Reflection.Metadata;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using server_api.Data.Models;
using server_api.Data.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using Microsoft.EntityFrameworkCore;
using server_api.Data.Models.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace server_api.Controllers
{
    public class GenreViewModel
    {
        public long total { get; set; }
        public IEnumerable<Genre> records { get; set; }
        public string sortBy { get; set; }
        public string direction { get; set; } = "asc";
        public int page { get; set; } = 1;
        public int limit { get; set; } = 5;
    }
    // Так как при ошибках валидации возвращается ошибки в секции errors то при добавлении ошибок вручную их нужно передовать 
    // с помощью ValidationProblem(ModelState)
    // Иначе вручную добавленные ошибки нужно будет искать в js по другому

    [ApiController]
    [Route(Auth.Constants.Strings.General.ApiDerictory + "/genres")]
    [Authorize]
    public class GenreApiController : ControllerBase
    {
        private readonly IGenreRepository genreRepository;

        public GenreApiController(IGenreRepository genreRepository)
        {
            this.genreRepository = genreRepository;
        }
        [HttpGet]
        public async Task<ActionResult<GenreViewModel>> GetGenreAsync
            (int? page, int? limit, string sortBy, string direction, string name)
        {
            int take = limit ?? 5;
            int skip = ((page ?? 1) - 1) * take;
            var records = genreRepository.ReadAll;
            direction = direction == "asc" ? "Ascending" : "Descending";

            if (!string.IsNullOrWhiteSpace(sortBy) && !string.IsNullOrWhiteSpace(direction))
            {
                records = records.Order(sortBy, direction);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                records = records.Where(it => EF.Functions.Like(it.Name, $"{name}%"));
            }
            int total = await records.CountAsync();
            var orderedRecords = await records.Skip(skip).Take(take).ToListAsync();

            var result = new GenreViewModel
            {
                total = total,
                records = orderedRecords
            };

            return (result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenreAsync
            (int id)
        {
            var genre = await genreRepository.Read(id);
            if (genre == null)
            {
                return NotFound();
            }
            return genre;
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "admins")]
        public async Task<IActionResult> PutGenreAsync(int id, [FromBody] Genre genre)
        {
            if (id != genre.Id)
            {
                ModelState.AddModelError("All", "Записи с таким Id не существует");
            }
            try
            {
                await genreRepository.Update(genre);
            }
            catch (Exception)
            {
                ModelState.AddModelError("All", "Ошибка записи - возможно такая запись уже существует");
            }
            return ModelState.IsValid ? NoContent() : ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(Roles = "admins")]
        public async Task<IActionResult> CreateGenre([FromBody] Genre genre)
        {
            try
            {
                await genreRepository.Create(genre);
            }
            catch (Exception)
            {
                ModelState.AddModelError("All", "Ошибка записи - возможно такая запись уже существует");
                return ValidationProblem(ModelState);
            }
            return CreatedAtAction(nameof(GetGenreAsync), new { id = genre.Id }, genre);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admins")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await genreRepository.Read(id);
            if (genre == null) return NotFound();
            await genreRepository.Delete(id);
            return NoContent();
        }
    }
}
