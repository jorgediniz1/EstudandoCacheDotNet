﻿using EstudandoCacheDotNet.Entities;
using EstudandoCacheDotNet.Infrastructure.Caching;
using EstudandoCacheDotNet.Infrastructure.Persistence;
using EstudandoCacheDotNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EstudandoCacheDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly ToDoListDbContext _context;
        private readonly ICachingService _cache;
        public ToDosController(ICachingService cache, ToDoListDbContext context)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var todoCache = await _cache.GetAsync(id.ToString());

            ToDo? todo;

            if (!string.IsNullOrWhiteSpace(todoCache))
            {
                todo = JsonConvert.DeserializeObject<ToDo>(todoCache);

                Console.WriteLine("Carregado do Cache");

                return Ok(todo);
            }

            todo = await _context.ToDos.SingleOrDefaultAsync(t => t.Id == id);

            if (todo is null)
            {
                return NotFound();
            }

            await _cache.SetAsync(id.ToString(), JsonConvert.SerializeObject(todo));

            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ToDoInputModel model)
        {
            var todo = new ToDo(0, model.Title, model.Description);

            await _context.ToDos.AddAsync(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, model);
        }
    }
}
