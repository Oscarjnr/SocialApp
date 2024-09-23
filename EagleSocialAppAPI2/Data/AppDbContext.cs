using EagleSocialAppAPI2.Models;
using Microsoft.EntityFrameworkCore;

namespace EagleSocialAppAPI2.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<AppUser> AppUsers { get; set; }
    }
}
