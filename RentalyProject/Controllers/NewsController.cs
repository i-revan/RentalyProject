using Microsoft.AspNetCore.Mvc;

namespace RentalyProject.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
