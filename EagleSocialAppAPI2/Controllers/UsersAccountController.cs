using EagleSocialAppAPI2.Data;
using EagleSocialAppAPI2.DTOs;
using EagleSocialAppAPI2.Interfaces;
using EagleSocialAppAPI2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EagleSocialAppAPI2.Controllers
{
    public class UsersAccountController(AppDbContext dbContext, ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")] //usersaccount/register
        public async Task<ActionResult<AppUserDTO>> Register(AccountCreationDTO accountCreationDTO)
        {

            if (await UserNameExists(accountCreationDTO.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            var appUser = new AppUser
            {
                UserName = accountCreationDTO.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(accountCreationDTO.Password)),
                PasswordSalt = hmac.Key
            };

            dbContext.AppUsers.Add(appUser);
            await dbContext.SaveChangesAsync();
            return new AppUserDTO
            {
                Username = appUser.UserName,
                Token = tokenService.CreateToken(appUser),
            };
            
        }

        [HttpPost("login")] //usersaccount/login
        public async Task<ActionResult<AppUserDTO>> Login(LoginDTO loginDTO)
        {
            var appUser = await dbContext.AppUsers.FirstOrDefaultAsync(x => 
                x.UserName == loginDTO.Username.ToLower());
            if (appUser == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(appUser.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != appUser.PasswordHash[i]) return Unauthorized("Invalid password");
            }
            return new AppUserDTO
            {
                Username = appUser.UserName,
                Token = tokenService.CreateToken(appUser)
            };
        }

        private async Task<bool> UserNameExists(string username) //constraint in db to check if username already exist
        {
            return await dbContext.AppUsers.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }
    }
}
