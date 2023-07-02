using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Reservation> reservations = _context.Reservations
                .Include(r=>r.AppUser)
                .Include(r=>r.Car)
                .ThenInclude(r => r.Model).ThenInclude(m=>m.Marka)
                .AsEnumerable();
            return View(reservations);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Reservation reservation = await _context.Reservations.Where(r => r.Id == id)
                .Include(r => r.AppUser)
                .Include(r => r.Car)
            .ThenInclude(r => r.Model).ThenInclude(m=>m.Marka).FirstOrDefaultAsync();
            if (reservation is null) throw new NotFoundException("There is no reservation has this id or it was deleted");
            return View(reservation);
        }
        public async Task<IActionResult> Accept(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Reservation reservation = await _context.Reservations.Where(r => r.Id == id).Include(r => r.Car).FirstOrDefaultAsync();
            if (reservation is null) throw new NotFoundException("There is no reservation has this id or it was deleted");
            reservation.Status = true;
            reservation.Car.IsAvailable = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> Decline(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Reservation reservation = await _context.Reservations.Where(r => r.Id == id).FirstOrDefaultAsync();
            if (reservation is null) throw new NotFoundException("There is no reservation has this id or it was deleted");
            reservation.Status = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
