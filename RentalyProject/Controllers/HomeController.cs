using Microsoft.AspNetCore.Mvc;

namespace RentalyProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
