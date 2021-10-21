using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoList.Models;
using TodoList.Models.ViewModel;
using TodoList.Repository.Base;

namespace TodoList.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TodoController : ControllerBase
    {
        #region .::Constructor
        private readonly AppDbContext context;
        public TodoController(AppDbContext context)
        {
            this.context = context;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetTaskAsync() => Ok(await context.Todo.AsNoTracking().Include(u => u.User).ToListAsync());

        [HttpPost]
        public async Task<IActionResult> PostTaskAsync([FromBody] CreateTodoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var todo = new Todo
                {
                    Title = model.Title,
                    UserId = model.UserId
                };

                await context.Todo.AddAsync(todo);
                await context.SaveChangesAsync();
                return Created($"api/v1/{todo.TodoId}", todo);
            }
            catch (System.Exception e)
            {

                return BadRequest(e.ToString());
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTaskAsync([FromRoute] int id)
        {


            try
            {
                var todo = await context.Todo.FirstOrDefaultAsync(u => u.TodoId == id);

                if (todo == null)
                    return NoContent();

                context.Todo.Remove(todo);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (System.Exception)
            {

                return Conflict();
            }
        }
    }
}