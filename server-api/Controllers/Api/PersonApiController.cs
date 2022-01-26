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

    public class PersonViewModel
    {
        public long total { get; set; }
        public IEnumerable<Person> records { get; set; }
        public string sortBy { get; set; }
        public string direction { get; set; } = "asc";
        public int page { get; set; } = 1;
        public int limit { get; set; } = 5;
    }
    // Так как при ошибках валидации возвращается ошибки в секции errors то при добавлении ошибок вручную их нужно передовать 
    // с помощью ValidationProblem(ModelState)
    // Иначе вручную добавленные ошибки нужно будет искать в js по другому

    [ApiController]
    [Route(Auth.Constants.Strings.General.ApiDerictory + "/persons")]
    [Authorize]
    public class PersonApiController : ControllerBase
    {
        private readonly IPersonRepository personRepository;

        public PersonApiController(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }
        [HttpGet]
        public async Task<ActionResult<PersonViewModel>> GetPersonsAsync
            (int? page, int? limit, string sortBy, string direction, string firstName, string secondName, string lastName)
        {
            int take = limit ?? 5;
            int skip = ((page ?? 1) - 1) * take;
            var records = personRepository.ReadAll;
            
            direction = direction == "asc" ? "Ascending" : "Descending";

            if (!string.IsNullOrWhiteSpace(sortBy) && !string.IsNullOrWhiteSpace(direction))
            {
                records = records.Order(sortBy, direction);
            }

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                records = records.Where(it =>EF.Functions.Like(it.FirstName,$"%{firstName}%") );
            }

            if (!string.IsNullOrWhiteSpace(secondName))
            {
                records = records.Where(it =>EF.Functions.Like(it.SecondName,$"%{secondName}%") );
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                records = records.Where(it =>EF.Functions.Like(it.LastName,$"%{lastName}%") );
            }
            int total = await records.CountAsync();
            var orderedRecords = await records.Skip(skip).Take(take).ToListAsync();

            var result = new PersonViewModel
            {
                total = total,
                records = orderedRecords
            };

            return (result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPersonAsync
            (int id)
        {
            var person = await personRepository.Read(id);
            if (person == null)
            {
                return NotFound();
            }
            return person;
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "admins")]
        public async Task<IActionResult> PutPersonAsync(int id, [FromBody] Person person)
        {
            if (id != person.Id)
            {
                ModelState.AddModelError("FirstName", "Записи с таким Id не существует");
            }
            try
            {
                await personRepository.Update(person);
            }
            catch (Exception)
            {
                ModelState.AddModelError("FirstName", "Ошибка записи - возможно такая запись уже существует");
            }
            return ModelState.IsValid ? NoContent() : ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(Roles = "admins")]
        public async Task<IActionResult> CreatePerson([FromBody] Person person)
        {
            try
            {
                await personRepository.Create(person);
            }
            catch (Exception)
            {
                ModelState.AddModelError("FirstName", "Ошибка записи - возможно такая запись уже существует");
                return ValidationProblem(ModelState);
            }
            return CreatedAtAction(nameof(GetPersonAsync), new { id = person.Id }, person);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admins")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await personRepository.Read(id);
            if (person == null) return NotFound();
            await personRepository.Delete(id);
            return NoContent();
        }
    }
}
