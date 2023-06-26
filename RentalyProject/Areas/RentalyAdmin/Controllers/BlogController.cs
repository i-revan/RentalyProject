using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.Blogs;
using System.Data;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private static readonly string _folder = @"assets/images/testimonial";

        public BlogController(AppDbContext context,IMapper mapper,IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        public IActionResult Index(int take = 3, int page = 1)
        {
            IEnumerable<Blog> blogs = _context.Blogs.Skip((page - 1) * take).Take(take);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.Blogs.Count() / take);
            ViewBag.CurrentPage = page;
            return View(blogs);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogVM blogVM)
        {
            if(!ModelState.IsValid) return View(blogVM);
            Blog blog = _mapper.Map<Blog>(blogVM);
            if (!blogVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "The file type must be image");
                return View();
            }
            if (!blogVM.Photo.CheckFileSize(300))
            {
                ModelState.AddModelError("Photo", "The file size must be less than or equal to 300 kb.");
                return View();
            }
            blog.ImageUrl = await blogVM.Photo.CreateFileAsync(_env.WebRootPath, _folder);
            blog.CreatedAt = DateTime.Now;
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Blog blog = await _context.Blogs.Where(b=>b.Id == id).FirstOrDefaultAsync();
            if (blog is null) throw new NotFoundException("There is no blog has this id or it was deleted");
            UpdateBlogVM updateBlogVM = _mapper.Map<UpdateBlogVM>(blog);
            return View(updateBlogVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,UpdateBlogVM blogVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Blog blog = await _context.Blogs.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (blog is null) throw new NotFoundException("There is no blog has this id or it was deleted");
            if (!ModelState.IsValid) return View(blogVM);
            _mapper.Map(blogVM, blog);
            if (!(blogVM.Photo is null))
            {
                if (!blogVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File format is not valid");
                    return View(blogVM);
                }
                if (!blogVM.Photo.CheckFileSize(300))
                {
                    ModelState.AddModelError("Photo", "The file size must be less than or equal to 300 kb.");
                    return View(blogVM);
                }
                blog.ImageUrl.DeleteFile(_env.WebRootPath, _folder);
                blog.ImageUrl = await blogVM.Photo.CreateFileAsync(_env.WebRootPath, _folder);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Blog blog = await _context.Blogs.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (blog is null) throw new NotFoundException("There is no blog has this id or it was deleted");
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Blog blog = await _context.Blogs.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (blog is null) throw new NotFoundException("There is no blog has this id or it was deleted");
            return View(blog);
        }
    }
}
