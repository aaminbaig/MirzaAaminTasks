using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Task2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoTaskController : ControllerBase
    {

        private readonly TaskDbContext _dbContext;

        public TodoTaskController(TaskDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllData()
        {
            var data = await _dbContext.TodoTable.ToListAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(Todo todoItem)
        {
            if (todoItem == null) return BadRequest();
            _dbContext.TodoTable.Add(todoItem);
            await _dbContext.SaveChangesAsync();
            
            return Ok(await _dbContext.TodoTable.ToListAsync());
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetDataById(Guid Id)
        {
            var todoItem = await _dbContext.TodoTable.FindAsync(Id);
            if (todoItem == null)
            {
                return NotFound("Item not found");
            }
            return Ok(todoItem);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateData(Todo item)
        {
            var todoItem = await _dbContext.TodoTable.FindAsync(item.Id);
            if (todoItem == null)
            {
                return NotFound("Item not found");
            }

            todoItem.Title = item.Title;
            todoItem.IsCompleted = item.IsCompleted;
            todoItem.UpdatedDate = item.UpdatedDate;
            todoItem.CreatedDate = item.CreatedDate;

            await _dbContext.SaveChangesAsync();

            return Ok(await _dbContext.TodoTable.ToListAsync());
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteTask(Guid Id)
        {
            var todoItem = await _dbContext.TodoTable.FindAsync(Id);
            if (todoItem == null)
            {
                return NotFound("Item not found");
            }

            _dbContext.TodoTable.Remove(todoItem);
            await _dbContext.SaveChangesAsync();
            return Ok(todoItem);
        }

    }
}
