﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Repositories.Interfaces;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.Markas;
using RentalyProject.ViewModels.Models;
using System.Data;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles = "Admin")]
    public class ModelController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IModelRepository _modelRepository;

        public ModelController(AppDbContext context,IMapper mapper,IModelRepository modelRepository)
        {
            _context = context;
            _mapper = mapper;
            _modelRepository = modelRepository;
        }
        public IActionResult Index(int take = 3,int page = 1)
        {

            IEnumerable<Model> models = _context.Models.Skip((page - 1) * take).Take(take).Include(m=>m.Marka);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.Models.Count() / take);
            ViewBag.CurrentPage = page;
            return View(models);
        }
        public IActionResult Create()
        {
            ViewBag.Markas = _context.Markas.AsEnumerable();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ModelVM modelVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Markas = _context.Markas.AsEnumerable();
                return View(modelVM);
            }
            if (await _context.Models.AnyAsync(m => m.Name.Trim().ToLower() == modelVM.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "There already exists model has this name");
                ViewBag.Markas = _context.Markas.AsEnumerable();
                return View(modelVM);
            }
            if (!(await _context.Markas.AnyAsync(m => m.Id == modelVM.MarkaId)))
            {
                ModelState.AddModelError("MarkaId", "There is no marka has this id or it was deleted");
                ViewBag.Markas = _context.Markas.AsEnumerable();
                return View(modelVM);
            }
            modelVM.Name = modelVM.Name.Capitalize();
            Model model = _mapper.Map<Model>(modelVM);
            model.CreatedAt = DateTime.Now;
            await _modelRepository.AddAsync(model);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Model model = await _context.Models.FirstOrDefaultAsync(m => m.Id == id);
            if (model is null) throw new NotFoundException("There is no model has this id or it was deleted");
            ViewBag.Markas = _context.Markas.AsEnumerable();
            ModelVM modelVM = _mapper.Map<ModelVM>(model);
            return View(modelVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,ModelVM modelVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Model model = await _context.Models.FirstOrDefaultAsync(m => m.Id == id);
            if (model is null) throw new NotFoundException("There is no model has this id or it was deleted");
            if (await _context.Models.AnyAsync(m => m.Name.Trim().ToLower() == modelVM.Name.Trim().ToLower() && m.Id!=id) )
            {
                ModelState.AddModelError("Name", "There already exists model has this name");
                ViewBag.Markas = _context.Markas.AsEnumerable();
                return View(modelVM);
            }
            if (!(await _context.Markas.AnyAsync(m => m.Id == modelVM.MarkaId)))
            {
                ModelState.AddModelError("MarkaId", "There is no marka has this id or it was deleted");
                ViewBag.Markas = _context.Markas.AsEnumerable();
                return View(modelVM);
            }
            model.Name = modelVM.Name.Capitalize();
            model.MarkaId = modelVM.MarkaId;
            model.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1) throw new BadRequestException("Id is not found");
            Model model = await _modelRepository.GetByIdAsync(id);
            if (model is null) throw new NotFoundException("There is no model has this id or it was deleted");
            await _modelRepository.DeleteAsync(model);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Model model = await _context.Models.Include(m=>m.Marka).FirstOrDefaultAsync(m => m.Id == id);
            if (model is null) throw new NotFoundException("There is no model has this id or it was deleted");
            return View(model);
        }
    }
}
