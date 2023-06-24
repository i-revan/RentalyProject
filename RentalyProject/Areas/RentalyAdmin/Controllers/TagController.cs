using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.Tags;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]

    public class TagController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TagController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index(int take = 3, int page = 1)
        {
            IEnumerable<Tag> tags = _context.Tags.Skip((page - 1) * take).Take(take);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.Tags.Count() / take);
            ViewBag.CurrentPage = page;
            return View(tags);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TagVM tagVM)
        {
            if (!ModelState.IsValid) return View(tagVM);
            Tag tag = _mapper.Map<Tag>(tagVM);
            tag.CreatedAt = DateTime.Now;
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Tag tag = await _context.Tags.Where(t=>t.Id==id).FirstOrDefaultAsync();
            if (tag is null) throw new NotFoundException("There is no tag has this id or it was deleted");
            TagVM tagVM = _mapper.Map<TagVM>(tag);
            return View(tagVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,TagVM tagVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Tag tag = await _context.Tags.Where(t => t.Id == id).FirstOrDefaultAsync();
            if (tag is null) throw new NotFoundException("There is no tag has this id or it was deleted");
            if (!ModelState.IsValid) return View(tagVM);
            _mapper.Map(tagVM, tag);
            tag.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Tag tag = await _context.Tags.Where(t => t.Id == id).FirstOrDefaultAsync();
            if (tag is null) throw new NotFoundException("There is no tag has this id or it was deleted");
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Tag tag = await _context.Tags.Where(t => t.Id == id).FirstOrDefaultAsync();
            if (tag is null) throw new NotFoundException("There is no tag has this id or it was deleted");
            return View(tag);
        }
    }
}
