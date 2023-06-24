using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.ViewModels;

namespace RentalyProject.Controllers
{
    [Authorize]
    public class RentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public RentController(AppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            //AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            //if (user is null) throw new NotFoundException("User is not found");
            return View(car);
        }
    }
}
