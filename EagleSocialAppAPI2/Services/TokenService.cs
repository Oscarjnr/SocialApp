using EagleSocialAppAPI2.Interfaces;
using EagleSocialAppAPI2.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EagleSocialAppAPI2.Services
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        public string CreateToken(AppUser appUser)
        {
            var tokenKey = configuration["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appsettings");
            if (tokenKey.Length < 64) throw new Exception("Make your tokenKey longer");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, appUser.UserName)
            };

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
