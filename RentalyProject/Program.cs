using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentalyProject.BackgroundServices;
using RentalyProject.DAL;
using RentalyProject.Interfaces;
using RentalyProject.Middlewares;
using RentalyProject.Models;
using RentalyProject.Repositories.Implementations;
using RentalyProject.Repositories.Interfaces;
using RentalyProject.Services;
using RentalyProject.Validators;
using Stripe;

namespace RentalyProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<RegisterValidator>());
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(50);
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

                opt.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            builder.Services.AddScoped<LayoutService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
            builder.Services.AddScoped<IModelRepository, ModelRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IBodyTypeRepository, BodyTypeRepository>();
            builder.Services.AddScoped<IMarkaRepository, MarkaRepository>();
            builder.Services.AddScoped<IFuelTypeRepository, FuelTypeRepository>();
            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<ITransmissionRepository, TransmissionRepository>();
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
            builder.Services.AddScoped<ReservationService>();
            builder.Services.AddHostedService<ReservationCompletionBackgroundService>();
            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            //app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:Secretkey"];
            
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