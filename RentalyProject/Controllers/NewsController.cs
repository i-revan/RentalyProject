using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.ViewModels;

namespace RentalyProject.Controllers
{
    public class NewsController : Controller
    {
        private readonly AppDbContext _context;

        public NewsController(AppDbContext context)
        {
            _context = context;
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
            NewsVM newsVM = new NewsVM()
            {
                News = _context.News.AsEnumerable(),
                Tags = _context.Tags.AsEnumerable(),
                news = news
            };
            return View(newsVM);
        }
    }
}
