using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.BodyTypes;
using RentalyProject.ViewModels.Positions;
using System.Data;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    public class PositionController : Controller
    {
        private readonly AppDbContext _context;

        public PositionController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int take = 3, int page = 1)
        {
            IEnumerable<Position> positions = _context.Positions.Skip((page - 1) * take).Take(take);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.Positions.Count() / take);
            ViewBag.CurrentPage = page;
            return View(positions);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PositionVM positionVM)
        {
            if (!ModelState.IsValid) return View();
            Position position = new Position()
            {
                Name = positionVM.Name.Capitalize(),
                CreatedAt = DateTime.Now
            };
            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Position position = await _context.Positions.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (position is null) throw new NotFoundException("There is no position has this id or it was deleted");
            PositionVM positionVM = new PositionVM()
            {
                Name = position.Name
            };
            return View(positionVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, PositionVM positionVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Position position = await _context.Positions.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (position is null) throw new NotFoundException("There is no position has this id or it was deleted");
            if (!ModelState.IsValid) return View(positionVM);
            if (await _context.Positions.AnyAsync(p => p.Name == positionVM.Name && p.Id != id))
            {
                ModelState.AddModelError("Name", "There already exists a position has this name");
                return View(positionVM);
            }
            position.Name = positionVM.Name.Capitalize();
            position.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Position position = await _context.Positions.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (position is null) throw new NotFoundException("There is no position has this id or it was deleted");
            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Position position = await _context.Positions.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (position is null) throw new NotFoundException("There is no position has this id or it was deleted");
            return View(position);
        }
    }
}
