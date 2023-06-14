using Microsoft.AspNetCore.Mvc;

namespace RentalyProject.Controllers
{
    public class CarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            return View();
        }
    }
}
