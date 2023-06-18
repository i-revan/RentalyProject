using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.Markas;
using System.Data;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles = "Admin")]
    public class MarkaController : Controller
    {
        private readonly AppDbContext _context;

        public MarkaController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int take = 3,int page=1)
        {
            IEnumerable<Marka> markas = _context.Markas.Skip((page-1)*take).Take(take);
            ViewBag.TotalPage = (int)Math.Ceiling((double) _context.Markas.Count()/ take);
            ViewBag.CurrentPage = page;
            return View(markas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MarkaVM markaVM)
        {
            if(!ModelState.IsValid) return View(markaVM);
            if(await _context.Markas.AnyAsync(m=>m.Name.Trim().ToLower() == markaVM.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "There already exists marka has this name");
                return View(markaVM);
            }
            Marka marka = new Marka()
            {
                Name = markaVM.Name.Capitalize(),
                CreatedAt = DateTime.Now
            };
            await _context.Markas.AddAsync(marka);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Marka marka = await _context.Markas.FirstOrDefaultAsync(m => m.Id == id);
            if (marka is null) throw new NotFoundException("Marka is not found");
            MarkaVM markaVM = new MarkaVM()
            {
                Name = marka.Name
            };
            return View(markaVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,MarkaVM markaVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Marka marka = await _context.Markas.FirstOrDefaultAsync(m => m.Id == id);
            if (marka is null) throw new NotFoundException("Marka is not found");
            if (await _context.Markas.AnyAsync(m => m.Name.Trim().ToLower() == markaVM.Name.Trim().ToLower() && m.Id!=marka.Id))
            {
                ModelState.AddModelError("Name", "There already exists marka has this name");
                return View(markaVM);
            }
            if (!ModelState.IsValid) return View(markaVM);
            marka.Name = markaVM.Name.Capitalize();
            marka.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Marka marka = await _context.Markas.Include(m=>m.Models).FirstOrDefaultAsync(m => m.Id == id);
            if (marka is null) throw new NotFoundException("There is no marka has this id or it was deleted");
            foreach(Model model in marka.Models)
            {
                _context.Models.Remove(model);
            }
            _context.Markas.Remove(marka);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Marka marka = await _context.Markas.FirstOrDefaultAsync(f => f.Id == id);
            if (marka is null) throw new NotFoundException("There is no marka has this id or it was deleted");
            return View(marka);
        }
    }
}
