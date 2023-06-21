using Microsoft.AspNetCore.Mvc;

namespace RentalyProject.Controllers
{
    public class RentController : Controller
    {
        public IActionResult Details(int? id)
        {
            return View();
        }
    }
}
