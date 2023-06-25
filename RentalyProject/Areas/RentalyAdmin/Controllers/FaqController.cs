using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.ViewModels.Faqs;
using System.Data;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    public class FaqController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FaqController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index(int take = 3, int page = 1)
        {
            IEnumerable<Faq> faqs = _context.Faqs.Skip((page - 1) * take).Take(take);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.Faqs.Count() / take);
            ViewBag.CurrentPage = page;
            return View(faqs);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(FaqVM faqVM)
        {
            if(!ModelState.IsValid) return View(faqVM);
            Faq faq = _mapper.Map<Faq>(faqVM);
            faq.CreatedAt = DateTime.Now;
            await _context.Faqs.AddAsync(faq);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Faq faq = await _context.Faqs.Where(f=>f.Id == id).FirstOrDefaultAsync();
            if (faq is null) throw new NotFoundException("There is no question has this id or it was deleted");
            FaqVM faqVM = _mapper.Map<FaqVM>(faq);
            return View(faqVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,FaqVM faqVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Faq faq = await _context.Faqs.Where(f => f.Id == id).FirstOrDefaultAsync();
            if (faq is null) throw new NotFoundException("There is no question has this id or it was deleted");
            if(!ModelState.IsValid) return View(faqVM);
            _mapper.Map(faqVM,faq);
            faq.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Faq faq = await _context.Faqs.Where(f => f.Id == id).FirstOrDefaultAsync();
            if (faq is null) throw new NotFoundException("There is no question has this id or it was deleted");
            _context.Faqs.Remove(faq);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Faq faq = await _context.Faqs.Where(f => f.Id == id).FirstOrDefaultAsync();
            if (faq is null) throw new NotFoundException("There is no question has this id or it was deleted");
            return View(faq);
        }
    }
}
