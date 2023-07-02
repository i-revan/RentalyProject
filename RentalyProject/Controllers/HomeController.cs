using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.ViewModels;

namespace RentalyProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(AppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                BodyTypes = _context.BodyTypes.AsEnumerable(),
                Services = _context.Services.AsEnumerable(),
                Blogs = _context.Blogs.AsEnumerable(),
                Faqs = _context.Faqs.AsEnumerable(),
                News = _context.News.AsEnumerable(),
                Cars = _context.Cars
                .Include(c => c.Model).ThenInclude(m=>m.Marka)
                .Include(c => c.CarImages)
                .Include(c => c.BodyType)
                .AsEnumerable()
            };
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user is null) throw new NotFoundException("User is not found");
                homeVM.UserFavorites = await _context.FavoriteCars.Where(fc=>fc.AppUserid==user.Id).ToListAsync();
            }
            
            return View(homeVM);
        }
        public IActionResult Cars(string? categoryName,string? bodyTypeName,int? seats,int? engineCapacity)
        {
            IQueryable<Car> query = _context.Cars
                .Include(c => c.Model).ThenInclude(m=>m.Marka)
                .Include(c => c.CarImages)
                .Include(c => c.BodyType).AsQueryable();

            if(!String.IsNullOrEmpty(categoryName))
            {
                query = query.Where(c=>c.Category.Name.Trim().ToLower() == categoryName.Trim().ToLower());
            }
            if (!String.IsNullOrEmpty(bodyTypeName))
            {
                query = query.Where(c => c.BodyType.Name.Trim().ToLower() == bodyTypeName.Trim().ToLower());
            }
            if (seats != null)
            {
                query = query.Where(c => c.Seats == seats);
            }

            IEnumerable<Car> Cars = _context.Cars
                .Include(c => c.Model).ThenInclude(m=>m.Marka)
                .Include(c => c.CarImages)
                .Include(c => c.BodyType)
                .AsEnumerable();
            CarsVM carsVM = new CarsVM
            {
                Cars = query.AsEnumerable(),
                Categories = _context.Categories.AsEnumerable(),
                BodyTypes = _context.BodyTypes.AsEnumerable()
            };
            return View(carsVM);
        }
        [Authorize]
        public async Task<IActionResult> Orders()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            List<Reservation> reservations = await _context.Reservations.Where(r => r.AppUser == user)
                .Include(r=>r.Car)
                .ThenInclude(c => c.Model)
                .ThenInclude(m=>m.Marka).ToListAsync();
            return View(reservations);
        }
    }
}
