using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Repositories.Interfaces;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.ViewModels.FuelTypes;
using RentalyProject.ViewModels.Transmissions;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    public class TransmissionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ITransmissionRepository _transmissionRepository;
        private readonly IMapper _mapper;

        public TransmissionController(AppDbContext context, ITransmissionRepository transmissionRepository,IMapper mapper)
        {
            _context = context;
            _transmissionRepository = transmissionRepository;
            _mapper = mapper;
        }
        public IActionResult Index(int take = 3, int page = 1)
        {
            IEnumerable<Transmission> transmissions = _context.Transmissions.Skip((page - 1) * take).Take(take);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.Transmissions.Count() / take);
            ViewBag.CurrentPage = page;
            return View(transmissions);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TransmissionVM transmissionVM)
        {
            if (!ModelState.IsValid) return View(transmissionVM);
            bool result = await _context.Transmissions.AnyAsync(t => t.Name.Trim().ToLower() == transmissionVM.Name.ToLower());
            if (result)
            {
                ModelState.AddModelError("Name", "There already exists a transmission has this name");
                return View(transmissionVM);
            }
            Transmission transmission = _mapper.Map<Transmission>(transmissionVM);
            transmission.CreatedAt = DateTime.Now;
            await _transmissionRepository.AddAsync(transmission);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id < 1) throw new BadRequestException("Id is not found");
            Transmission transmission = await _transmissionRepository.GetByIdAsync(id);
            if (transmission == null) throw new NotFoundException("There is no transmission has this id or it was deleted");
            TransmissionVM transmissionVM = _mapper.Map<TransmissionVM>(transmission);
            return View(transmissionVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(TransmissionVM transmissionVM, int id)
        {
            if (id < 1) throw new BadRequestException("Id is not found");
            Transmission transmission = await _transmissionRepository.GetByIdAsync(id);
            if (transmission == null) throw new NotFoundException("There is no transmission has this id or it was deleted");
            bool result = await _context.FuelTypes.AnyAsync(t => t.Name.Trim().ToLower() == transmissionVM.Name.ToLower() && t.Id != transmission.Id);
            if (result)
            {
                ModelState.AddModelError("Name", "There already exists a transmission has this name");
                return View(transmissionVM);
            }
            if (!ModelState.IsValid) return View(transmissionVM);
            _mapper.Map(transmissionVM, transmission);
            transmission.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1) throw new BadRequestException("Id is not found");
            Transmission transmission = await _transmissionRepository.GetByIdAsync(id);
            if (transmission == null) throw new NotFoundException("There is no transmission has this id or it was deleted");
            await _transmissionRepository.DeleteAsync(transmission);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id < 1) throw new BadRequestException("Id is not found");
            Transmission transmission = await _transmissionRepository.GetByIdAsync(id);
            if (transmission == null) throw new NotFoundException("There is no transmission has this id or it was deleted");

            return View(transmission);
        }
    }
}
