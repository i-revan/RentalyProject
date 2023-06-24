using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.ViewModels.Services;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles ="Admin")]
    [AutoValidateAntiforgeryToken]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ServiceController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index(int page = 1,int take=3)
        {
            IEnumerable<Service> services = _context.Services.Skip((page - 1) * take).Take(take);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.Services.Count() / take);
            ViewBag.CurrentPage = page;
            return View(services);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ServiceVM serviceVM)
        {
            if(!ModelState.IsValid) return View(serviceVM);
            Service service = _mapper.Map<Service>(serviceVM);
            service.CreatedAt = DateTime.Now;
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Service service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service is null) throw new NotFoundException("There is no service has this id or it was deleted");
            ServiceVM serviceVM = _mapper.Map<ServiceVM>(service);
            return View(serviceVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,ServiceVM serviceVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Service service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service is null) throw new NotFoundException("There is no service has this id or it was deleted");
            if (!ModelState.IsValid) return View(serviceVM);
            _mapper.Map(serviceVM,service);
            service.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Service service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service is null) throw new NotFoundException("There is no service has this id or it was deleted");
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Service service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service is null) throw new NotFoundException("There is no service has this id or it was deleted");
            return View(service);
        }
    }
}
