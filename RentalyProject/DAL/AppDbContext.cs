using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentalyProject.Models;

namespace RentalyProject.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> option):base(option)
        {
            
        }
        public DbSet<Setting> Settings { get; set; }
    }
}
