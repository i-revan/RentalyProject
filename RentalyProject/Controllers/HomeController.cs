using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.ViewModels;

namespace RentalyProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                BodyTypes =  _context.BodyTypes.AsEnumerable(),
                Cars = _context.Cars
                .Include(c=>c.Marka)
                .Include(c=>c.CarImages)
                .Include(c=>c.BodyType)
                .AsEnumerable()
            };
            return View(homeVM);
        }
        public IActionResult Favorite()
        {
            return View();
        }
    }
}
