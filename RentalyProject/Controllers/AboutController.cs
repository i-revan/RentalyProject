using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.ViewModels.UserQuestions;

namespace RentalyProject.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AboutController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(UserQuestionVM userQuestionVM)
        {
            if(!ModelState.IsValid) return View(userQuestionVM);
            UserQuestion userQuestion = _mapper.Map<UserQuestion>(userQuestionVM);
            userQuestion.CreatedAt = DateTime.Now;
            await _context.UserQuestions.AddAsync(userQuestion);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
