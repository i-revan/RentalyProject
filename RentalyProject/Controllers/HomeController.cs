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
                .Include(c => c.Marka)
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
        public IActionResult Cars()
        {
            IEnumerable<Car> Cars = _context.Cars
                .Include(c => c.Marka)
                .Include(c => c.CarImages)
                .Include(c => c.BodyType)
                .AsEnumerable();
            return View(Cars);
        }
        [Authorize]
        public async Task<IActionResult> Orders()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<Reservation> reservations = await _context.Reservations.Where(r => r.AppUser == user)
                .Include(r=>r.Car)
                .ThenInclude(c=>c.Marka).ToListAsync();
            return View(reservations);
        }
    }
}
