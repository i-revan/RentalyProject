using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.ViewModels;
using RentalyProject.ViewModels.Comments;

namespace RentalyProject.Controllers
{
    public class NewsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public NewsController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index(int page=1,int take=6)
        {
            IEnumerable<News> news = _context.News.Skip((page - 1) * take).Take(take);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.News.Count() / take);
            ViewBag.CurrentPage = page;
            NewsVM newsVM = new NewsVM()
            {
                News = news,
                NewsForPosts = _context.News.AsEnumerable(),
                Tags = _context.Tags.AsEnumerable()
            };
            return View(newsVM);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            News news = await _context.News.Where(n => n.Id == id)
                .Include(n => n.NewsTags)
                .ThenInclude(nt=>nt.Tag).FirstOrDefaultAsync();
            if (news is null) throw new NotFoundException("There is no news has this id or it was deleted");
            news.Comments = await _context.Comments.Where(c=>c.NewsId ==  id).ToListAsync();
            NewsVM newsVM = new NewsVM()
            {
                News = _context.News.AsEnumerable(),
                Tags = _context.Tags.AsEnumerable(),
                news = news
            };
            
            return View(newsVM);
        }
        [HttpPost]
        public async Task<IActionResult> Details(int? id,NewsVM newsVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            News news = await _context.News.Where(n => n.Id == id)
                .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag).FirstOrDefaultAsync();
            if (news is null) throw new NotFoundException("There is no news has this id or it was deleted");
            
            if (!ModelState.IsValid)
            {
                NewsVM returnNews = new NewsVM()
                {
                    News = _context.News.AsEnumerable(),
                    Tags = _context.Tags.AsEnumerable(),
                    news = news
                };
                return View(returnNews);
            }
            if(newsVM.commentVM.Name is null)
            {

                NewsVM returnNews = new NewsVM()
                {
                    News = _context.News.AsEnumerable(),
                    Tags = _context.Tags.AsEnumerable(),
                    news = news
                };
                news.Comments = await _context.Comments.Where(c => c.NewsId == id).ToListAsync();
                ModelState.AddModelError(String.Empty, "Enter your name");
                return View(returnNews);
            }
            if (newsVM.commentVM.Surname is null)
            {

                NewsVM returnNews = new NewsVM()
                {
                    News = _context.News.AsEnumerable(),
                    Tags = _context.Tags.AsEnumerable(),
                    news = news
                };
                news.Comments = await _context.Comments.Where(c => c.NewsId == id).ToListAsync();
                ModelState.AddModelError(String.Empty, "Enter your surname");
                return View(returnNews);
            }
            if (newsVM.commentVM.Email is null)
            {

                NewsVM returnNews = new NewsVM()
                {
                    News = _context.News.AsEnumerable(),
                    Tags = _context.Tags.AsEnumerable(),
                    news = news
                };
                news.Comments = await _context.Comments.Where(c => c.NewsId == id).ToListAsync();
                ModelState.AddModelError(String.Empty, "Enter your email");
                return View(returnNews);
            }
            if (newsVM.commentVM.Message is null)
            {

                NewsVM returnNews = new NewsVM()
                {
                    News = _context.News.AsEnumerable(),
                    Tags = _context.Tags.AsEnumerable(),
                    news = news
                };
                news.Comments = await _context.Comments.Where(c => c.NewsId == id).ToListAsync();
                ModelState.AddModelError(String.Empty, "Enter your message");
                return View(returnNews);
            }
            Comment comment = _mapper.Map<Comment>(newsVM.commentVM);
            comment.NewsId = news.Id;
            comment.CreatedAt = DateTime.Now;
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
