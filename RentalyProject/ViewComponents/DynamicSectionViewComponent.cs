using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;

namespace RentalyProject.ViewComponents
{
    public class DynamicSectionViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;

        public DynamicSectionViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            DynamicSection dynamicSection = await _context.DynamicSections.FirstOrDefaultAsync();
            return View(dynamicSection);
        }
    }
}
