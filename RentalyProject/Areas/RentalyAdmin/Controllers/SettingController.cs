using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.ViewModels.Settings;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles ="Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
            return View(settings);
        }
        public async Task<IActionResult> Update(string key)
        {
            if (key == null) return BadRequest();
            var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Key == key);
            UpdateSettingVM settingVM = new UpdateSettingVM()
            {
                Value = setting.Value
            };
            return View(settingVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(string key, UpdateSettingVM settingVM)
        {
            if (key == null) return BadRequest();
            var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Key == key);
            setting.Value = settingVM.Value;
            setting.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
