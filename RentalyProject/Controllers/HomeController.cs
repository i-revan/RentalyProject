using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Mvc;
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
                BodyTypes =  _context.BodyTypes.AsEnumerable()
            };
            return View(homeVM);
        }
        public IActionResult Favorite()
        {
            return View();
        }
    }
}
