using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.FuelTypes;
using System.Data;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles = "Admin")]
    public class FuelTypeController : Controller
    {
        private readonly AppDbContext _context;

        public FuelTypeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int take=3,int page=1)
        {
            IEnumerable<FuelType> fuelTypes = _context.FuelTypes.Skip((page-1)*take).Take(take);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.FuelTypes.Count() / take);
            ViewBag.CurrentPage = page;
            return View(fuelTypes);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(FuelTypeVM fueltypeVM)
        {
            if(!ModelState.IsValid) return View(fueltypeVM);
            bool result = await _context.FuelTypes.AnyAsync(f=>f.Name.Trim().ToLower() == fueltypeVM.Name.ToLower());
            if (result)
            {
                ModelState.AddModelError("Name", "There already exists a fuel type has this name");
                return View(fueltypeVM);
            }
            FuelType fuel = new FuelType()
            {
                Name = fueltypeVM.Name.Capitalize(),
                CreatedAt = DateTime.Now
            };
            await _context.FuelTypes.AddAsync(fuel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) throw new BadRequestException("Id is not found");
            FuelType fuel = await _context.FuelTypes.FirstOrDefaultAsync(f=>f.Id==id);
            if (fuel == null) throw new NotFoundException("There is no fuel type has this id or it was deleted");
            FuelTypeVM fuelTypeVM = new FuelTypeVM()
            {
                Name = fuel.Name
            };
            return View(fuelTypeVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(FuelTypeVM fueltypeVM,int? id)
        {
            if (id == null || id < 1) throw new BadRequestException("Id is not found");
            FuelType fuel = await _context.FuelTypes.FirstOrDefaultAsync(f => f.Id == id);
            if (fuel == null) throw new NotFoundException("There is no fuel type has this id or it was deleted");
            bool result = await _context.FuelTypes.AnyAsync(f => f.Name.Trim().ToLower() == fueltypeVM.Name.ToLower() && f.Id!=fuel.Id);
            if (result)
            {
                ModelState.AddModelError("Name", "There already exists a fuel type has this name");
                return View(fueltypeVM);
            }
            if (!ModelState.IsValid) return View(fueltypeVM);
            fuel.Name = fueltypeVM.Name.Capitalize();
            fuel.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id<1) throw new BadRequestException("Id is not found");
            FuelType fuel = await _context.FuelTypes.FirstOrDefaultAsync(f=>f.Id == id);
            if (fuel is null) throw new NotFoundException("There is no fuel type has this id or it was deleted");
            _context.FuelTypes.Remove(fuel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id < 1) throw new BadRequestException("Id is not found");
            FuelType fuel = await _context.FuelTypes.FirstOrDefaultAsync(f => f.Id == id);
            if (fuel is null) throw new NotFoundException("There is no fuel type has this id or it was deleted");

            return View(fuel);
        }
    }
}
