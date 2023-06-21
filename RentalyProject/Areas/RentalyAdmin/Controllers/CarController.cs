using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.Cars;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles ="Admin")]
    [AutoValidateAntiforgeryToken]
    public class CarController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private static readonly string _folder = @"assets/images/cars";

        public CarController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int take=3,int page=1)
        {
            IEnumerable<Car> cars = _context.Cars.Skip((page - 1) * take).Take(take).Include(c=>c.Marka);
            ViewBag.TotalPage = (int)Math.Ceiling((double)_context.Cars.Count() / take);
            ViewBag.CurrentPage = page;
            return View(cars);
        }
        public IActionResult Create()
        {
            ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
            ViewBag.Markas = _context.Markas.AsEnumerable();
            ViewBag.Features = _context.Features.AsEnumerable();
            ViewBag.Colors = _context.Colors.AsEnumerable();
            ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
            ViewBag.Categories = _context.Categories.AsEnumerable();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCarVM carVM)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                ViewBag.Markas = _context.Markas.AsEnumerable();
                ViewBag.Features = _context.Features.AsEnumerable();
                ViewBag.Colors = _context.Colors.AsEnumerable();
                ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                return View();
            }
            Car car = new Car()
            {
                Seats = carVM.Seats,
                Doors = carVM.Doors,
                Luggage = carVM.Luggage,
                EngineCapacity = carVM.EngineCapacity,
                Year = carVM.Year,
                Milleage = carVM.Milleage,
                Transmission = carVM.Transmission,
                FuelEconomy = carVM.FuelEconomy,
                RentPrice = carVM.RentPrice,
                Description = carVM.Description,
                FuelTypeId = carVM.FuelTypeId,
                MarkaId = carVM.MarkaId,
                BodyTypeId = carVM.BodyTypeId,
                CategoryId = carVM.CategoryId,
                IsAvailable = false,
                Like = 0,
                CreatedAt = DateTime.Now,
                CarFeatures = new List<CarFeature>(),
                CarColors = new List<CarColor>(),
                CarImages = new List<CarImages>()
            };
            foreach (int featureId in carVM.FeatureIds)
            {
                if (!(await _context.Features.AnyAsync(b => b.Id == featureId)))
                {
                    ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                    ViewBag.Markas = _context.Markas.AsEnumerable();
                    ViewBag.Features = _context.Features.AsEnumerable();
                    ViewBag.Colors = _context.Colors.AsEnumerable();
                    ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                    ModelState.AddModelError("FeatureIds", $"There is no feature that has {featureId} id");
                    return View();
                }
                CarFeature carFeature = new CarFeature()
                {
                    FeatureId = featureId,
                    CreatedAt = DateTime.Now,
                    Car = car
                };
                car.CarFeatures.Add(carFeature);
            }
            for(int i = 0; i < carVM.ColorIds.Count; i++)
            {
                if (!(await _context.Colors.AnyAsync(b => b.Id == carVM.ColorIds[i])))
                {
                    ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                    ViewBag.Markas = _context.Markas.AsEnumerable();
                    ViewBag.Features = _context.Features.AsEnumerable();
                    ViewBag.Colors = _context.Colors.AsEnumerable();
                    ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                    ModelState.AddModelError("FeatureIds", $"There is no color that has {carVM.ColorIds[i]} id");
                    return View();
                }
                if (i == 0)
                {
                    CarColor carColor = new CarColor()
                    {
                        ColorId = carVM.ColorIds[i],
                        CreatedAt = DateTime.Now,
                        Car = car,
                        IsInterior = true
                    };
                    car.CarColors.Add(carColor);
                }
                else
                {
                    CarColor carColor = new CarColor()
                    {
                        ColorId = carVM.ColorIds[i],
                        CreatedAt = DateTime.Now,
                        Car = car,
                        IsInterior = false
                    };
                    car.CarColors.Add(carColor);
                }
            }
            if(carVM.MainPhoto != null)
            {
                if (!carVM.MainPhoto.CheckFileType("image/"))
                {
                    ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                    ViewBag.Markas = _context.Markas.AsEnumerable();
                    ViewBag.Features = _context.Features.AsEnumerable();
                    ViewBag.Colors = _context.Colors.AsEnumerable();
                    ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                    ModelState.AddModelError("MainPhoto", "File format is not correct!");
                    return View();
                }
                if (!carVM.MainPhoto.CheckFileSize(500))
                {
                    ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                    ViewBag.Markas = _context.Markas.AsEnumerable();
                    ViewBag.Features = _context.Features.AsEnumerable();
                    ViewBag.Colors = _context.Colors.AsEnumerable();
                    ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                    ModelState.AddModelError("MainPhoto", "File size must be 500 kb or less!");
                    return View();
                }
                CarImages carImage = new CarImages()
                {
                    ImageUrl = await carVM.MainPhoto.CreateFileAsync(_env.WebRootPath,_folder),
                    Car = car,
                    IsMain = true,
                    CreatedAt = DateTime.Now,
                };
                car.CarImages.Add(carImage);
            }
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Car car = await _context.Cars.Where(c => c.Id == id)
                .Include(c=>c.Marka)
                .Include(c=>c.BodyType)
                .Include(c=>c.FuelType)
                .Include(c=>c.Category)
                .Include(c=>c.CarColors).ThenInclude(cc=>cc.Color)
                .Include(c=>c.CarFeatures).ThenInclude(cf=>cf.Feature)
                .Include(c=>c.CarImages)
                .FirstOrDefaultAsync();
            if (car is null) throw new NotFoundException("There is no car has this id or it was deleted");
            return View(car);
        }
    }
}
