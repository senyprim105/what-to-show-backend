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
using Microsoft.AspNetCore.Http;

namespace server_api.Controllers
{
    public class SimpleViewModel<T, K>
    where T : ISimpleEntity<K>
    where K : IEquatable<K>
    {
        public long total { get; set; }
        public IEnumerable<T> records { get; set; }
        public string sortBy { get; set; }
        public string direction { get; set; } = "asc";
        public int page { get; set; } = 1;
        public int limit { get; set; } = 5;
    }
    // Так как при ошибках валидации возвращается ошибки в секции errors то при добавлении ошибок вручную их нужно передовать 
    // с помощью ValidationProblem(ModelState)
    // Иначе вручную добавленные ошибки нужно будет искать в js по другому

    [ApiController]
    [Route(Auth.Constants.Strings.General.ApiDerictory + "/reviews")]
    [Authorize]
    public class GenericApiController<T, K> : ControllerBase
    where T : ISimpleEntity<K>
    where K : IEquatable<K>
    {
        private readonly ISimpleRepository<T, K> repository;

        public GenericApiController(ISimpleRepository<T, K> repository)
        {
            this.repository = repository;
        }
        // Сортирует 
        virtual protected IQueryable<T> SortAsync(IQueryable<T> records, string sortBy, string direction)
        {
            if (!string.IsNullOrWhiteSpace(sortBy) && !string.IsNullOrWhiteSpace(direction))
            {
                direction = direction == "asc" ? "Ascending" : "Descending";
                records = records.Order(sortBy, direction);
            }
            return records;
        }
        // Создаем запрос фильтрации IQuarable по именам и значениям полей
        // В switch  долны быть варианты фильтрации (чтобы не писать динамический запрос)
        // Если для имени 
        // -есть одно значение то ищутся поля начинающиеся с него 
        // -если несколько значений то ищутся поля совпадающие с ними
        // Необходимо переопределять в каждом наследнике
        virtual protected IQueryable<T> FilterRecords(IQueryable<T> records, Dictionary<string, string[]> filters)
        {
            foreach (var entry in filters)
            {
                //Если значение пустышка - не фильтровать по нему
                if ( entry.Value.Length==0) continue;
                switch (entry.Key)
                {
                    case "name":
                        records = entry.Value.Length > 1
           ? records.Where(record => entry.Value.Contains(record.Name, StringComparer.OrdinalIgnoreCase))
           : records.Where(record => EF.Functions.Like(record.Name, $"{entry.Value[0]}%"));
                        break;
                    default: break;
                }
            }
            return records;
        }
        virtual protected IQueryable<T> Pagination(IQueryable<T> records, int? page, int? limit)
        {
            int take = limit ?? 5;
            int skip = ((page ?? 1) - 1) * take;
            return records.Skip(skip).Take(take);
        }

        //Выбирает параметры из строки запроса соответствующие архиву имен и выводит в словаре
        virtual protected Dictionary<string, string[]> GetQueryParameters(IQueryCollection queryParams, string[] filterNames=null)
        =>   filterNames==null
        ? queryParams.ToDictionary(param => param.Key, param => param.Value.ToArray())
        : queryParams
            .Where(param => !filterNames.Contains(param.Key, StringComparer.OrdinalIgnoreCase))
            .ToDictionary(param => param.Key, param => param.Value.Where(it=>!string.IsNullOrWhiteSpace(it)).ToArray());

        //Выборка записей
        [HttpGet]
        virtual public async Task<ActionResult<SimpleViewModel<T, K>>> GetAsync
            (int? page, int? limit, string sortBy, string direction)
        {
            //Берем все записи
            var records = repository.ReadAll;
            //Фильтруем 
            records = FilterRecords(records,GetQueryParameters(Request.Query));
            //Сортируем
            records = SortAsync(records,sortBy,direction);
            //Считаем колличество отфильтрованных записей
            int total = await records.CountAsync();
            //Отсекаем пагинацией
            records = Pagination(records,page,limit);

            
            var resultRecords = await records.ToListAsync();
            var result = new SimpleViewModel<T, K>
            {
                total = total,
                records = resultRecords
            };

            return (result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<T>> GetAsync
            (K id)
        {
            var entity = await repository.Read(id);
            if (entity == null)
            {
                return NotFound();
            }
            return entity;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admins")]
        public async Task<IActionResult> PutAsync(K id, [FromBody] T entity)
        {
            if (!id.Equals(entity.Id))
            {
                ModelState.AddModelError("All", "Записи с таким Id не существует");
            }
            try
            {
                await repository.Update(entity);
            }
            catch (Exception)
            {
                ModelState.AddModelError("All", "Ошибка записи - возможно такая запись уже существует");
            }
            return ModelState.IsValid ? NoContent() : ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(Roles = "admins")]
        public async Task<IActionResult> CreateAsync([FromBody] T entity)
        {
            try
            {
                await repository.Create(entity);
            }
            catch (Exception)
            {
                ModelState.AddModelError("All", "Ошибка записи - возможно такая запись уже существует");
                return ValidationProblem(ModelState);
            }
            return CreatedAtAction(nameof(GetAsync), new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admins")]
        public async Task<IActionResult> DeleteAsync(K id)
        {
            var entity = await repository.Read(id);
            if (entity == null) return NotFound();
            await repository.Delete(id);
            return NoContent();
        }
    }
}
