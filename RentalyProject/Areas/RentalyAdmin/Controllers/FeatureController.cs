using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.Features;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles ="Admin")]
    [AutoValidateAntiforgeryToken]
    public class FeatureController : Controller
    {
        private readonly AppDbContext _context;

        public FeatureController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int take=3,int page=1)
        {
            IEnumerable<Feature> features = _context.Features.Skip((page - 1) * take).Take(take);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.Features.Count() / take);
            ViewBag.CurrentPage = page;
            return View(features);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(FeatureVM featureVM)
        {
            if(!ModelState.IsValid) return View(featureVM);
            if(await _context.Features.AnyAsync(f=>f.Name.Trim().ToLower() == featureVM.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "There already exists feature has same name");
                return View(featureVM);
            }
            Feature feature = new Feature()
            {
                Name = featureVM.Name.Capitalize(),
                CreatedAt = DateTime.Now
            };
            await _context.Features.AddAsync(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Feature feature = await _context.Features.FirstOrDefaultAsync(m => m.Id == id);
            if (feature is null) throw new NotFoundException("There is no feature has this id or it was deleted");
            FeatureVM featureVM = new FeatureVM()
            {
                Name = feature.Name
            };
            return View(featureVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,FeatureVM featureVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Feature feature = await _context.Features.FirstOrDefaultAsync(m => m.Id == id);
            if (feature is null) throw new NotFoundException("There is no feature has this id or it was deleted");
            if(!ModelState.IsValid) return View(featureVM);
            if (await _context.Features.AnyAsync(f => f.Name.Trim().ToLower() == featureVM.Name.Trim().ToLower() && f.Id!=id))
            {
                ModelState.AddModelError("Name", "There already exists feature has same name");
                return View(featureVM);
            }
            featureVM.Name = feature.Name.Capitalize();
            feature.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Feature feature = await _context.Features.FirstOrDefaultAsync(m => m.Id == id);
            if (feature is null) throw new NotFoundException("There is no feature has this id or it was deleted");
            _context.Features.Remove(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Feature feature = await _context.Features.FirstOrDefaultAsync(m => m.Id == id);
            if (feature is null) throw new NotFoundException("There is no feature has this id or it was deleted");
            return View(feature);
        }
    }
}
