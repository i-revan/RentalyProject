using Microsoft.AspNetCore.Mvc;

namespace RentalyProject.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult ErrorPage(string errorMessage="Error occured")
        {
            return View(model:errorMessage);
        }
    }
}
