using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.Blogs;
using RentalyProject.ViewModels.Employees;
using System.Data;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private static readonly string _folder = @"assets/images/team";

        public EmployeeController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        public IActionResult Index(int take = 3, int page = 1)
        {
            IEnumerable<Employee> employees = _context.Employees.Skip((page - 1) * take).Take(take).Include(e=>e.Position);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.Employees.Count() / take);
            ViewBag.CurrentPage = page;
            return View(employees);
        }
        public IActionResult Create()
        {
            ViewBag.Positions = _context.Positions;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM employeeVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = _context.Positions;
                return View(employeeVM);
            }
            Employee employee = _mapper.Map<Employee>(employeeVM);
            if (!employeeVM.Photo.CheckFileType("image/"))
            {
                ViewBag.Positions = _context.Positions;
                ModelState.AddModelError("Photo", "The file type must be image");
                return View();
            }
            if (!employeeVM.Photo.CheckFileSize(300))
            {
                ViewBag.Positions = _context.Positions;
                ModelState.AddModelError("Photo", "The file size must be less than or equal to 300 kb.");
                return View();
            }
            employee.ImageUrl = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, _folder);
            employee.CreatedAt = DateTime.Now;
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Employee employee = await _context.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (employee is null) throw new NotFoundException("There is no employee has this id or it was deleted");
            ViewBag.Positions = _context.Positions;
            UpdateEmployeeVM updateEmployeeVM = _mapper.Map<UpdateEmployeeVM>(employee);
            return View(updateEmployeeVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateEmployeeVM updateEmployeeVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Employee employee = await _context.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (employee is null) throw new NotFoundException("There is no employee has this id or it was deleted");
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = _context.Positions;
                return View(updateEmployeeVM);
            }
            _mapper.Map(updateEmployeeVM, employee);
            if (!(updateEmployeeVM.Photo is null))
            {
                if (!updateEmployeeVM.Photo.CheckFileType("image/"))
                {
                    ViewBag.Positions = _context.Positions;
                    ModelState.AddModelError("Photo", "File format is not valid");
                    return View(updateEmployeeVM);
                }
                if (!updateEmployeeVM.Photo.CheckFileSize(300))
                {
                    ViewBag.Positions = _context.Positions;
                    ModelState.AddModelError("Photo", "The file size must be less than or equal to 300 kb.");
                    return View(updateEmployeeVM);
                }
                employee.ImageUrl.DeleteFile(_env.WebRootPath, _folder);
                employee.ImageUrl = await updateEmployeeVM.Photo.CreateFileAsync(_env.WebRootPath, _folder);
            }
            employee.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Employee employee = await _context.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (employee is null) throw new NotFoundException("There is no employee has this id or it was deleted");
            employee.ImageUrl.DeleteFile(_env.WebRootPath, _folder);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Employee employee = await _context.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (employee is null) throw new NotFoundException("There is no employee has this id or it was deleted");
            return View(employee);
        }
    }
}
