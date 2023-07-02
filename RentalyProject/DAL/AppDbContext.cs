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
        public DbSet<Service> Services { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }
        public DbSet<DynamicSection> DynamicSections { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<UserQuestion> UserQuestions { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Transmission> Transmissions { get; set; }
    }
}
