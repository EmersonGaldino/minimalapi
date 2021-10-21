using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Models;
using TodoList.Models.ViewModel;
using TodoList.Repository.Base;

namespace TodoList.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        #region .::Constructor
        private readonly AppDbContext context;
        public UserController(AppDbContext context)
        {
            this.context = context;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetTaskAsync() => Ok(await context.User.AsNoTracking().Include(x => x.Todo).ToListAsync());

        [HttpPost]
        public async Task<IActionResult> PostTaskAsync([FromBody] CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var user = new User
                {
                    Email = model.Email,
                    Name = model.Name
                };


                await context.User.AddAsync(user);
                await context.SaveChangesAsync();
                return Created($"api/v1/{user.UserId}", user);
            }
            catch (System.Exception)
            {

                return Conflict();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTaskAsync([FromRoute] int id)
        {

            try
            {
                var user = await context.User.FirstOrDefaultAsync(u => u.UserId == id);

                if (user == null)
                    return NoContent();

                context.User.Remove(user);
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