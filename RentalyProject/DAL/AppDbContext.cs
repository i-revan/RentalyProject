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
        public DbSet<BodyType> BodyTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BodyTypeCategory> BodyTypeCategories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarColor> CarColors { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<CarFeature> CarFeatures { get; set; }
        public DbSet<CarImages> CarImages { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<Marka> Markas { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<FavoriteCar> FavoriteCars { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
