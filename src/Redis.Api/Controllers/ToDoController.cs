using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Redis.Api.Models;
using Redis.Cache;
using Redis.Entities;
using Redis.Persistence;

namespace Redis.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoController : ControllerBase
{
    private readonly ToDoListDbContext _context;
    private readonly IRedisService _cache;

    public ToDoController(ToDoListDbContext context, IRedisService cache)
    {
        _context = context;
        _cache = cache;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var todos = await _context.ToDos.ToListAsync();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        ToDo? todo;

        var toDoCache = await _cache.GetAsync(id.ToString());

        if (string.IsNullOrEmpty(toDoCache))
        {
            todo = await _context.ToDos.SingleOrDefaultAsync(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            await _cache.SetAsync(id.ToString(),JsonConvert.SerializeObject(todo));
        }
        else
        {
            todo = JsonConvert.DeserializeObject<ToDo>(toDoCache);
        }

        return Ok(todo);
    }

    [HttpPost]
    public async Task<IActionResult> Post(ToDoCreateModel model)
    {
        var todo = new ToDo(Guid.Empty,model.Title,model.Description);

        await _context.ToDos.AddAsync(todo);
        await _context.SaveChangesAsync();

        return Created(nameof(Post), todo);
    }
}