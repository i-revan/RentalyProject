using Microsoft.AspNetCore.Mvc;
using RentalyProject.DAL;
using RentalyProject.Models;

namespace RentalyProject.ViewComponents
{
    public class ServiceViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public ServiceViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<Service> services = _context.Services.AsEnumerable();
            return View(services);
        }
    }
}
