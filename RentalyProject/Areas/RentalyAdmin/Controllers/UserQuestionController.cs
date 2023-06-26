using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using System.Data;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    public class UserQuestionController : Controller
    {
        private readonly AppDbContext _context;
        public UserQuestionController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int take = 3, int page = 1)
        {
            IEnumerable<UserQuestion> userQuestions = _context.UserQuestions.Skip((page - 1) * take).Take(take);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.UserQuestions.Count() / take);
            ViewBag.CurrentPage = page;
            return View(userQuestions);
        }
    }
}
