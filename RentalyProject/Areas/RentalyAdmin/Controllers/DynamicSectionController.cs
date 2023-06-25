using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.DynamicSections;
using System.Data;
using static NuGet.Packaging.PackagingConstants;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    public class DynamicSectionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private static readonly string _folder = @"assets/images/background";

        public DynamicSectionController(AppDbContext context,IWebHostEnvironment env,IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            IEnumerable<DynamicSection> dynamicSections = _context.DynamicSections;
            return View(dynamicSections);
        }

        //Dinamik section'in ilk defe yaranmasi ucun
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Create(CreateDynamicSectionVM dynamicSectionVM)
        //{
        //    if(!ModelState.IsValid) return View(dynamicSectionVM);
        //    DynamicSection dynamicSection = _mapper.Map<DynamicSection>(dynamicSectionVM);
        //    if (!dynamicSectionVM.Photo.CheckFileType("image/"))
        //    {
        //        ModelState.AddModelError("Photo", "The file type must be image");
        //        return View();
        //    }
        //    if (!dynamicSectionVM.Photo.CheckFileSize(400))
        //    {
        //        ModelState.AddModelError("Photo", "The file size must be less than or equal to 400 kb.");
        //        return View();
        //    }
        //    dynamicSection.ImageUrl = await dynamicSectionVM.Photo.CreateFileAsync(_env.WebRootPath, _folder);
        //    dynamicSection.CreatedAt = DateTime.Now;
        //    await _context.DynamicSections.AddAsync(dynamicSection);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            DynamicSection section = await _context.DynamicSections.Where(ds=>ds.Id == id).FirstOrDefaultAsync();
            if (section is null) throw new NotFoundException("There is no section has this id or it was deleted");
            UpdateDynamicSectionVM dynamicSectionVM = _mapper.Map<UpdateDynamicSectionVM>(section);
            return View(dynamicSectionVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,UpdateDynamicSectionVM dynamicSectionVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            DynamicSection section = await _context.DynamicSections.Where(ds => ds.Id == id).FirstOrDefaultAsync();
            if (section is null) throw new NotFoundException("There is no section has this id or it was deleted");
            if (!ModelState.IsValid) return View(dynamicSectionVM);
            _mapper.Map(dynamicSectionVM, section);
            if(!(dynamicSectionVM.Photo is null))
            {
                if (!dynamicSectionVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "The file type must be image");
                    return View(dynamicSectionVM);
                }
                if (!dynamicSectionVM.Photo.CheckFileSize(400))
                {
                    ModelState.AddModelError("Photo", "The file size must be less than or equal to 400 kb.");
                    return View(dynamicSectionVM);
                }
                section.ImageUrl.DeleteFile(_env.WebRootPath, _folder);
                section.ImageUrl = await dynamicSectionVM.Photo.CreateFileAsync(_env.WebRootPath, _folder);
            }
            section.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            DynamicSection section = await _context.DynamicSections.Where(ds => ds.Id == id).FirstOrDefaultAsync();
            if (section is null) throw new NotFoundException("There is no section has this id or it was deleted");
            return View(section);
        }
    }
}
