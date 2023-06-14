using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Services;

namespace RentalyProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;

                opt.User.RequireUniqueEmail = true;

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            builder.Services.AddScoped<LayoutService>();
            builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area:exists}/{controller=home}/{action=index}/{id?}"
                );
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=home}/{action=index}/{id?}"
                );

            app.Run();
        }
    }
}