using EagleSocialAppAPI2.Data;
using EagleSocialAppAPI2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EagleSocialAppAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(AppDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var appUsers = await dbContext.AppUsers.ToListAsync();
            return appUsers;
        }

        [HttpGet("{id:int}")] //api/users/1
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var appUser = await dbContext.AppUsers.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }

            return appUser;
        }

    }
}
