using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Task2.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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
            var userId = User?.FindFirstValue("id");
            var userRole = User?.FindFirstValue(ClaimTypes.Role);
            var data = await _dbContext.TodoTable.ToListAsync();

            var finalData = data.Where(x => x.CreatedByUser.ToString() == userId).ToList();

            if (userRole == "Admin")
            {
                finalData = data;
            }
            return Ok(finalData);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(Todo todoItem)
        {
            if (todoItem == null) return BadRequest();

            var userId = User.FindFirstValue("id");
            todoItem.CreatedByUser = int.Parse(userId);
            _dbContext.TodoTable.Add(todoItem);
            await _dbContext.SaveChangesAsync();

            return Ok(await _dbContext.TodoTable.ToListAsync());
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetDataById(Guid Id)
        {
            var userId = User?.FindFirstValue("id");
            var userRole = User?.FindFirstValue(ClaimTypes.Role);
            var todoItem = await _dbContext.TodoTable.FindAsync(Id);

            if (todoItem == null)
            {
                return NotFound("Item not found");
            }

            if (userRole == "Admin")
            {
                return Ok(todoItem);
            }

            if (todoItem.CreatedByUser.ToString() != userId)
            {
                return NotFound("Item not found");
            }

            return Ok(todoItem);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateData(Todo item)
        {
            var userId = User?.FindFirstValue("id");
            var userRole = User?.FindFirstValue(ClaimTypes.Role);
            var todoItem = await _dbContext.TodoTable.FindAsync(item.Id);

            if (todoItem == null)
            {
                return NotFound("Item not found");
            }

            if (userRole == "Admin" || todoItem.CreatedByUser.ToString() == userId)
            {
                todoItem.Title = item.Title;
                todoItem.IsCompleted = item.IsCompleted;
                todoItem.UpdatedDate = item.UpdatedDate;
                todoItem.CreatedDate = item.CreatedDate;

                await _dbContext.SaveChangesAsync();

                return Ok("Item updated!!!");

            }
            else
            {
                return NotFound("Item not found");
            }

        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteTask(Guid Id)
        {
            var todoItem = await _dbContext.TodoTable.FindAsync(Id);
            var userRole = User?.FindFirstValue(ClaimTypes.Role);
            var userId = User?.FindFirstValue("id");
            if (todoItem == null)
            {
                return NotFound("Item not found");
            }

            if (userRole == "Admin" || todoItem.CreatedByUser.ToString() == userId)
            {
                _dbContext.TodoTable.Remove(todoItem);
                await _dbContext.SaveChangesAsync();
                return Ok("Successfully Deleted!!!");
            }
            else
            {
                return Ok("You do not have the right to delete this task!");
            }

        }

    }
}
