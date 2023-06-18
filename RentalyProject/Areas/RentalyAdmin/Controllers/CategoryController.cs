using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.BodyTypes;
using RentalyProject.ViewModels.Categories;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private static readonly string _folder = @"assets/images/select-form";
        public CategoryController(AppDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }
        public IActionResult Index(int take = 3, int page = 1)
        {
            IEnumerable<Category> categories = _context.Categories.Skip((page - 1) * take).Take(take);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.Categories.Count() / take);
            ViewBag.CurrentPage = page;
            return View(categories);
        }
        public IActionResult Create()
        {
            ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                return View();
            }
            Category category = new Category()
            {
                Name = categoryVM.Name.Capitalize(),
                BodyTypeCategories = new List<BodyTypeCategory>()
            };
            foreach (int bodyTypeId in categoryVM.BodyTypeIds)
            {
                if (!(await _context.BodyTypes.AnyAsync(b => b.Id == bodyTypeId)))
                {
                    ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                    ModelState.AddModelError("BodyTypeIds", $"There is no body type that has {bodyTypeId} id");
                    return View();
                }
                BodyTypeCategory bodyTypeCategory = new BodyTypeCategory()
                {
                    BodyTypeId = bodyTypeId,
                    Category = category
                };
                category.BodyTypeCategories.Add(bodyTypeCategory);
            }

            if (!categoryVM.Photo.CheckFileType("image/"))
            {
                ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                ModelState.AddModelError("Photo", "The file type must be image");
                return View();
            }
            if (!categoryVM.Photo.CheckFileSize(200))
            {
                ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                ModelState.AddModelError("Photo", "The file size must be less than or equal to 200 kb.");
                return View();
            }
            category.ImageUrl = await categoryVM.Photo.CreateFileAsync(_env.WebRootPath, _folder);
            category.CreatedAt = DateTime.Now;
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Category category = await _context.Categories.Where(c => c.Id == id).Include(c => c.BodyTypeCategories).FirstOrDefaultAsync();
            if (category is null) throw new NotFoundException("There is no category has this id or it was deleted");
            UpdateCategoryVM categoryVM = _mapper.Map<UpdateCategoryVM>(category);
            categoryVM.BodyTypeIds = category.BodyTypeCategories.Select(btc => btc.BodyTypeId).ToList();
            ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
            return View(categoryVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateCategoryVM categoryVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Category existed = await _context.Categories.Where(c => c.Id == id).Include(c => c.BodyTypeCategories).FirstOrDefaultAsync();
            if (existed is null) throw new NotFoundException("There is no category has this id or it was deleted");
            if (!ModelState.IsValid)
            {
                ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                return View(categoryVM);
            }
            List<int> createList = categoryVM.BodyTypeIds.Where(bt => !existed.BodyTypeCategories.Exists(btc => btc.BodyTypeId == bt)).ToList();
            foreach (int typeId in createList)
            {
                if (!(await _context.BodyTypes.AnyAsync(btc => btc.Id == typeId)))
                {
                    ModelState.AddModelError("BodyTypeIds", "There is no body type has this id or it was deleted");
                    ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                    return View(categoryVM);
                }
                BodyTypeCategory bodyTypeCategory = new BodyTypeCategory()
                {
                    CategoryId = existed.Id,
                    BodyTypeId = typeId
                };
                existed.BodyTypeCategories.Add(bodyTypeCategory);
            }
            List<BodyTypeCategory> removeList = existed.BodyTypeCategories.Where(btc => !categoryVM.BodyTypeIds.Contains(btc.BodyTypeId)).ToList();
            if (!(removeList is null))
            {
                _context.BodyTypeCategories.RemoveRange(removeList);
            }
            if (!(categoryVM.Photo is null))
            {
                if (!categoryVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File format is not valid");
                    ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                    return View(categoryVM);
                }
                if (!categoryVM.Photo.CheckFileSize(200))
                {
                    ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                    ModelState.AddModelError("Photo", "The file size must be less than or equal to 200 kb.");
                    return View(categoryVM);
                }
                existed.ImageUrl.DeleteFile(_env.WebRootPath, _folder);
                existed.ImageUrl = await categoryVM.Photo.CreateFileAsync(_env.WebRootPath, _folder);
            }
            existed.Name = categoryVM.Name.Capitalize();
            existed.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null) throw new NotFoundException("There is no category has this id or it was deleted");
            List<BodyTypeCategory> removeList = await _context.BodyTypeCategories.Where(b => b.CategoryId == id).ToListAsync();
            _context.BodyTypeCategories.RemoveRange(removeList);
            category.ImageUrl.DeleteFile(_env.WebRootPath, _folder);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Category category = await _context.Categories
                .Include(c => c.BodyTypeCategories)
                .ThenInclude(btc => btc.BodyType)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (category is null) throw new NotFoundException("There is no category has this id or it was deleted");
            return View(category);
        }
    }
}
