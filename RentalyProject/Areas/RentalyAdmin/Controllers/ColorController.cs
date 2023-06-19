using AutoMapper.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.Colors;
using RentalyProject.ViewModels.Features;
using System.Data;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;

        public ColorController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int take=3,int page=1)
        {
            IEnumerable<Color> colors = _context.Colors.Skip((page - 1) * take).Take(take);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.Colors.Count() / take);
            ViewBag.CurrentPage = page;
            return View(colors);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ColorVM colorVM)
        {
            if (!ModelState.IsValid) return View(colorVM);
            if (await _context.Colors.AnyAsync(f => f.Name.Trim().ToLower() == colorVM.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "There already exists color has same name");
                return View(colorVM);
            }
            Color color = new Color()
            {
                Name = colorVM.Name.Capitalize(),
                CreatedAt = DateTime.Now
            };
            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Color color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (color is null) throw new NotFoundException("There is no color has this id or it was deleted");
            ColorVM colorVM = new ColorVM()
            {
                Name = color.Name
            };
            return View(colorVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,ColorVM colorVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Color color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (color is null) throw new NotFoundException("There is no color has this id or it was deleted");
            if (!ModelState.IsValid) return View(colorVM);
            if (await _context.Colors.AnyAsync(f => f.Name.Trim().ToLower() == colorVM.Name.Trim().ToLower() && f.Id != id))
            {
                ModelState.AddModelError("Name", "There already exists feature has same name");
                return View(colorVM);
            }
            color.Name = colorVM.Name.Capitalize();
            color.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Color color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (color is null) throw new NotFoundException("There is no color has this id or it was deleted");
            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Color color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (color is null) throw new NotFoundException("There is no color has this id or it was deleted");

            return View(color);
        }
    }
}
