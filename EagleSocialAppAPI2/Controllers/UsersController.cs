using EagleSocialAppAPI2.Data;
using EagleSocialAppAPI2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EagleSocialAppAPI2.Controllers
{
    public class UsersController(AppDbContext dbContext) : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var appUsers = await dbContext.AppUsers.ToListAsync();
            return appUsers;
        }

        [Authorize]
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
