using EagleSocialAppAPI2.Models;

namespace EagleSocialAppAPI2.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
