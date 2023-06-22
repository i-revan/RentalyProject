using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;

namespace RentalyProject.Controllers
{
    public class RentController : Controller
    {
        private readonly AppDbContext _context;

        public RentController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Car car = await _context.Cars
                .Include(c=>c.Marka)
                .Include(c=>c.FuelType)
                .Include(c=>c.BodyType)
                .Include(c=>c.CarImages)
                .Include(c=>c.CarFeatures).ThenInclude(cf=>cf.Feature)
                .Include(c=>c.CarColors).ThenInclude(cc=>cc.Color)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (car is null) throw new NotFoundException("There is no car has this id or it was deleted");
            return View(car);
        }
    }
}
