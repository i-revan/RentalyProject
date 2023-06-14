using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
