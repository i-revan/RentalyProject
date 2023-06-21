using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.BodyTypes;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles ="Admin")]
    [AutoValidateAntiforgeryToken]
    public class BodyTypeController : Controller
    {
        private readonly AppDbContext _context;

        public BodyTypeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int take=3,int page=1)
        {
            IEnumerable<BodyType> bodyTypes = _context.BodyTypes.Skip((page - 1) * take).Take(take);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.BodyTypes.Count() / take);
            ViewBag.CurrentPage = page;
            return View(bodyTypes);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BodyTypeVM bodyTypeVM)
        {
            if(!ModelState.IsValid) return View();
            BodyType bodyType = new BodyType()
            {
                Name = bodyTypeVM.Name.Capitalize(),
                CreatedAt = DateTime.Now
            };
            await _context.BodyTypes.AddAsync(bodyType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            BodyType bodyType = await _context.BodyTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (bodyType is null) throw new NotFoundException("There is no body type has this id or it was deleted");
            BodyTypeVM bodyTypeVM = new BodyTypeVM()
            {
                Name = bodyType.Name
            };
            return View(bodyTypeVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,BodyTypeVM bodyTypeVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            BodyType bodyType = await _context.BodyTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (bodyType is null) throw new NotFoundException("There is no body type has this id or it was deleted");
            if(await _context.BodyTypes.AnyAsync(b=>b.Name == bodyTypeVM.Name && b.Id!=id))
            {
                ModelState.AddModelError("Name","There already exists a body type has this name");
                return View(bodyTypeVM);
            }
            bodyType.Name = bodyTypeVM.Name.Capitalize();
            bodyType.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            BodyType bodyType = await _context.BodyTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (bodyType is null) throw new NotFoundException("There is no body type has this id or it was deleted");
            _context.BodyTypes.Remove(bodyType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            BodyType bodyType = await _context.BodyTypes.FirstOrDefaultAsync(b => b.Id == id);
            if (bodyType is null) throw new NotFoundException("There is no body type has this id or it was deleted");
            return View(bodyType);
        }
    }
}
