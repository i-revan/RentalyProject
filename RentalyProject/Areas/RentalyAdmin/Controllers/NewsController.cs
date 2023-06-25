using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.Newss;
using System.Data;
using static NuGet.Packaging.PackagingConstants;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles = "Admin,ContentWriter")]
    [AutoValidateAntiforgeryToken]
    public class NewsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private static readonly string _folder = @"assets/images/news";

        public NewsController(AppDbContext context, UserManager<AppUser> userManager, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _env = env;
        }
        public IActionResult Index(int page = 1, int take = 3)
        {
            IEnumerable<News> news = _context.News.Skip((page - 1) * take).Take(take);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.News.Count() / take);
            ViewBag.CurrentPage = page;
            return View(news);
        }
        public IActionResult Create()
        {
            ViewBag.Tags = _context.Tags.AsEnumerable();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateNewsVM newsVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Tags = _context.Tags.AsEnumerable();
                return View(newsVM);
            }
            News news = _mapper.Map<News>(newsVM);
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            news.CreatedBy = user.Name;
            news.NewsTags = new List<NewsTag>();
            if (!(newsVM.TagIds is null))
            {
                foreach (int tagId in newsVM.TagIds)
                {
                    if (!(await _context.Tags.AnyAsync(t => t.Id == tagId)))
                    {
                        ViewBag.Tags = _context.Tags.AsEnumerable();
                        ModelState.AddModelError("TagIds", $"There is no body type that has {tagId} id");
                        return View(newsVM);
                    }
                    NewsTag newsTag = new NewsTag()
                    {
                        TagId = tagId,
                        News = news,
                        CreatedAt = DateTime.Now
                    };
                    news.NewsTags.Add(newsTag);
                }
            }
            if (!newsVM.Photo.CheckFileType("image/"))
            {
                ViewBag.Tags = _context.Tags.AsEnumerable();
                ModelState.AddModelError("Photo", "The file type must be image");
                return View(newsVM);
            }
            if (!newsVM.Photo.CheckFileSize(300))
            {
                ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                ModelState.AddModelError("Photo", "The file size must be less than or equal to 300 kb.");
                return View(newsVM);
            }
            news.ImageUrl = await newsVM.Photo.CreateFileAsync(_env.WebRootPath, _folder);
            news.CreatedAt = DateTime.Now;
            await _context.News.AddAsync(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            News news = await _context.News.Where(n => n.Id == id)
                .Include(n => n.NewsTags).FirstOrDefaultAsync();
            if (news is null) throw new NotFoundException("There is no news has this id or it was deleted");
            UpdateNewsVM newsVM = _mapper.Map<UpdateNewsVM>(news);

            ViewBag.Tags = _context.Tags.AsEnumerable();

            return View(newsVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateNewsVM newsVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            News existed = await _context.News.Where(n => n.Id == id)
                .Include(n => n.NewsTags).FirstOrDefaultAsync();
            if (existed is null) throw new NotFoundException("There is no news has this id or it was deleted");
            if (!ModelState.IsValid)
            {
                ViewBag.Tags = _context.Tags.AsEnumerable();
                return View(newsVM);
            }
            _mapper.Map(newsVM, existed);
            if (!(newsVM.TagIds is null))
            {
                List<int> createList = newsVM.TagIds.Where(t => !existed.NewsTags.Exists(nt => nt.TagId == t)).ToList();
                if (!(createList is null))
                {
                    foreach (int tagId in createList)
                    {
                        if (!(await _context.Tags.AnyAsync(t => t.Id == tagId)))
                        {
                            ModelState.AddModelError("TagIds", "There is no tag has this id or it was deleted");
                            ViewBag.Tags = _context.Tags.AsEnumerable();
                            return View(newsVM);
                        }
                        NewsTag newsTag = new NewsTag()
                        {
                            NewsId = existed.Id,
                            TagId = tagId,
                            CreatedAt = DateTime.Now
                        };
                        existed.NewsTags.Add(newsTag);
                    }
                }
                List<NewsTag> removeList = existed.NewsTags.Where(nt => !newsVM.TagIds.Contains(nt.TagId)).ToList();
                if (!(removeList is null))
                {
                    _context.NewsTags.RemoveRange(removeList);
                }
                if (!(newsVM.Photo is null))
                {
                    if (!newsVM.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File format is not valid");
                        ViewBag.Tags = _context.Tags.AsEnumerable();
                        return View(newsVM);
                    }
                    if (!newsVM.Photo.CheckFileSize(300))
                    {
                        ViewBag.Tags = _context.Tags.AsEnumerable();
                        ModelState.AddModelError("Photo", "The file size must be less than or equal to 300 kb.");
                        return View(newsVM);
                    }
                    existed.ImageUrl.DeleteFile(_env.WebRootPath, _folder);
                    existed.ImageUrl = await newsVM.Photo.CreateFileAsync(_env.WebRootPath, _folder);
                }
            }

           
            existed.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            News news = await _context.News.Where(n => n.Id == id)
                .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag).FirstOrDefaultAsync();
            if (news is null) throw new NotFoundException("There is no news has this id or it was deleted");
            List<NewsTag> removeList = await _context.NewsTags.Where(nt => nt.NewsId == news.Id).ToListAsync();
            _context.NewsTags.RemoveRange(removeList);
            news.ImageUrl.DeleteFile(_env.WebRootPath, _folder);
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            News news = await _context.News.Where(n => n.Id == id)
                .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag).FirstOrDefaultAsync();
            if (news is null) throw new NotFoundException("There is no news has this id or it was deleted");
            return View(news);
        }
    }
}
