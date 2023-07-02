using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Repositories.Interfaces;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.Cars;

namespace RentalyProject.Areas.RentalyAdmin.Controllers
{
    [Area("RentalyAdmin")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    public class CarController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ICarRepository _carRepository;
        private static readonly string _folder = @"assets/images/cars";

        public CarController(AppDbContext context, IWebHostEnvironment env,ICarRepository carRepository)
        {
            _context = context;
            _env = env;
            _carRepository = carRepository;
        }
        public IActionResult Index(int take = 3, int page = 1)
        {
            IEnumerable<Car> cars = _context.Cars.Skip((page - 1) * take).Take(take)
                .Include(c => c.Model).ThenInclude(m=>m.Marka)
                .Include(c => c.CarImages)
                .Include(c => c.CarFeatures).ThenInclude(cf => cf.Feature);
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
            if (!ModelState.IsValid)
            {
                ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                ViewBag.Markas = _context.Markas.AsEnumerable();
                ViewBag.Features = _context.Features.AsEnumerable();
                ViewBag.Colors = _context.Colors.AsEnumerable();
                ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                ViewBag.Categories = _context.Categories.AsEnumerable();

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
                ModelId = carVM.ModelId,
                //MarkaId = carVM.MarkaId,
                BodyTypeId = carVM.BodyTypeId,
                CategoryId = carVM.CategoryId,
                IsAvailable = true,
                Like = 0,
                CreatedAt = DateTime.Now,
                CarFeatures = new List<CarFeature>(),
                CarColors = new List<CarColor>(),
                CarImages = new List<CarImages>()
            };
            if (!(carVM.FeatureIds is null))
            {
                foreach (int featureId in carVM.FeatureIds)
                {
                    if (!(await _context.Features.AnyAsync(b => b.Id == featureId)))
                    {
                        ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                        ViewBag.Markas = _context.Markas.AsEnumerable();
                        ViewBag.Features = _context.Features.AsEnumerable();
                        ViewBag.Colors = _context.Colors.AsEnumerable();
                        ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                        ViewBag.Categories = _context.Categories.AsEnumerable();

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
            }

            for (int i = 0; i < carVM.ColorIds.Count; i++)
            {
                if (!(await _context.Colors.AnyAsync(b => b.Id == carVM.ColorIds[i])))
                {
                    ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                    ViewBag.Markas = _context.Markas.AsEnumerable();
                    ViewBag.Features = _context.Features.AsEnumerable();
                    ViewBag.Colors = _context.Colors.AsEnumerable();
                    ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                    ViewBag.Categories = _context.Categories.AsEnumerable();

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

            //Esas seklin yoxlanilmasi ve yaradilmasi

            if (!carVM.MainPhoto.CheckFileType("image/"))
            {
                ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                ViewBag.Markas = _context.Markas.AsEnumerable();
                ViewBag.Features = _context.Features.AsEnumerable();
                ViewBag.Colors = _context.Colors.AsEnumerable();
                ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                ViewBag.Categories = _context.Categories.AsEnumerable();

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
                ViewBag.Categories = _context.Categories.AsEnumerable();

                ModelState.AddModelError("MainPhoto", "File size must be 500 kb or less!");
                return View();
            }
            CarImages carImage = new CarImages()
            {
                ImageUrl = await carVM.MainPhoto.CreateFileAsync(_env.WebRootPath, _folder),
                Car = car,
                IsMain = true,
                CreatedAt = DateTime.Now,
            };
            car.CarImages.Add(carImage);
            if (carVM.Photos != null)
            {
                TempData["PhotoErrors"] = "";
                foreach (IFormFile photo in carVM.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        TempData["PhotoErrors"] += $"{photo.FileName} has not valid file format\t";
                        continue;
                    }
                    if (!photo.CheckFileSize(500))
                    {
                        TempData["PhotoErors"] += $"{photo.FileName} has not valid length";
                        continue;
                    }
                    CarImages additional = new CarImages()
                    {
                        ImageUrl = await photo.CreateFileAsync(_env.WebRootPath, _folder),
                        Car = car,
                        IsMain = false,
                        CreatedAt = DateTime.Now,
                    };
                    car.CarImages.Add(additional);
                }
            }
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Car car = await _context.Cars.Where(c => c.Id == id)
                .Include(c => c.CarFeatures)
                .Include(c => c.CarImages)
                .FirstOrDefaultAsync();
            if (car is null) throw new NotFoundException("There is no car has this id or it was deleted");
            UpdateCarVM carVM = new UpdateCarVM()
            {
                Seats = car.Seats,
                Doors = car.Doors,
                Luggage = car.Luggage,
                EngineCapacity = car.EngineCapacity,
                Year = car.Year,
                Milleage = car.Milleage,
                Transmission = car.Transmission,
                FuelEconomy = car.FuelEconomy,
                RentPrice = car.RentPrice,
                Description = car.Description,
                CategoryId = car.CategoryId,
                //MarkaId = car.MarkaId,
                BodyTypeId = car.BodyTypeId,
                FuelTypeId = car.FuelTypeId,
                FeatureIds = car.CarFeatures.Select(cf => cf.FeatureId).ToList()
            };
            carVM = carVM.MapImages(car);
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
            ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
            ViewBag.Markas = _context.Markas.AsEnumerable();
            ViewBag.Features = _context.Features.AsEnumerable();
            return View(carVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateCarVM carVM)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Car existed = await _context.Cars.Where(c => c.Id == id)
                .Include(c => c.CarFeatures)
                .Include(c => c.CarImages)
                .FirstOrDefaultAsync();
            if (existed is null) throw new NotFoundException("There is no car has this id or it was deleted");
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.AsEnumerable();
                ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                ViewBag.Markas = _context.Markas.AsEnumerable();
                ViewBag.Features = _context.Features.AsEnumerable();
                carVM = carVM.MapImages(existed);
                return View(carVM);
            }
            if (!(await _context.Categories.AnyAsync(c => c.Id == carVM.CategoryId)))
            {
                ViewBag.Categories = _context.Categories.AsEnumerable();
                ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                ViewBag.Markas = _context.Markas.AsEnumerable();
                ViewBag.Features = _context.Features.AsEnumerable();
                ViewBag.Colors = _context.Colors.AsEnumerable();
                carVM = carVM.MapImages(existed);
                ModelState.AddModelError("CategoryId", "There is no category has this id or ot was deleted");
                return View(carVM);
            }
            if (!(await _context.BodyTypes.AnyAsync(bt => bt.Id == carVM.BodyTypeId)))
            {
                ViewBag.Categories = _context.Categories.AsEnumerable();
                ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                ViewBag.Markas = _context.Markas.AsEnumerable();
                ViewBag.Features = _context.Features.AsEnumerable();
                ViewBag.Colors = _context.Colors.AsEnumerable();
                carVM = carVM.MapImages(existed);
                ModelState.AddModelError("BodyTypeid", "There is no body type has this id or ot was deleted");
                return View(carVM);
            }
            if (!(await _context.FuelTypes.AnyAsync(ft => ft.Id == carVM.FuelTypeId)))
            {
                ViewBag.Categories = _context.Categories.AsEnumerable();
                ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                ViewBag.Markas = _context.Markas.AsEnumerable();
                ViewBag.Features = _context.Features.AsEnumerable();
                ViewBag.Colors = _context.Colors.AsEnumerable();
                carVM = carVM.MapImages(existed);
                ModelState.AddModelError("FuelTypeId", "There is no fuel type has this id or ot was deleted");
                return View(carVM);
            }
            
            existed.Seats = carVM.Seats;
            existed.Doors = carVM.Doors;
            existed.Luggage = carVM.Luggage;
            existed.Transmission = existed.Transmission;
            existed.RentPrice = carVM.RentPrice;
            existed.Milleage = carVM.Milleage;
            existed.Year = carVM.Year;
            existed.Description = carVM.Description;
            existed.EngineCapacity = carVM.EngineCapacity;
            existed.FuelEconomy = carVM.FuelEconomy;
            existed.CategoryId = carVM.CategoryId;
            existed.FuelTypeId = carVM.FuelTypeId;
            existed.BodyTypeId = carVM.BodyTypeId;

            if (carVM.MainPhoto != null)
            {
                if (!carVM.MainPhoto.CheckFileType("image/"))
                {
                    ViewBag.Categories = _context.Categories.AsEnumerable();
                    ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                    ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                    ViewBag.Markas = _context.Markas.AsEnumerable();
                    ViewBag.Features = _context.Features.AsEnumerable();
                    ViewBag.Colors = _context.Colors.AsEnumerable();
                    carVM = carVM.MapImages(existed);
                    ModelState.AddModelError("MainPhoto", "Invalid file format");
                    return View(carVM);
                }
                if (!carVM.MainPhoto.CheckFileSize(500))
                {
                    ViewBag.Categories = _context.Categories.AsEnumerable();
                    ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                    ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                    ViewBag.Markas = _context.Markas.AsEnumerable();
                    ViewBag.Features = _context.Features.AsEnumerable();
                    ViewBag.Colors = _context.Colors.AsEnumerable();
                    carVM = carVM.MapImages(existed);
                    ModelState.AddModelError("MainPhoto", "Photo length must be or less than 500 kb");
                    return View(carVM);
                }
                var mainImage = existed.CarImages.FirstOrDefault(ci => ci.IsMain == true);
                mainImage.ImageUrl.DeleteFile(_env.WebRootPath, _folder);
                existed.CarImages.Remove(mainImage);
                CarImages carImage = new CarImages()
                {
                    CarId = existed.Id,
                    ImageUrl = await carVM.MainPhoto.CreateFileAsync(_env.WebRootPath, _folder),
                    IsMain = true
                };
                existed.CarImages.Add(carImage);
            }

            List<CarImages> removePhotoList = existed.CarImages.Where(ci => !carVM.ImageIds.Contains(ci.Id) && ci.IsMain == false).ToList();
            foreach (CarImages carImage in removePhotoList)
            {
                carImage.ImageUrl.DeleteFile(_env.WebRootPath, _folder);
                existed.CarImages.Remove(carImage);
            }
            if (!(carVM.Photos is null))
            {
                TempData["PhotoErrors"] = "";
                foreach (IFormFile photo in carVM.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        TempData["PhotoErrors"] += $"{photo.FileName} has not valid file format\t";
                        continue;
                    }
                    if (!photo.CheckFileSize(500))
                    {
                        TempData["PhotoErors"] += $"{photo.FileName} has not valid length";
                        continue;
                    }
                    CarImages additional = new CarImages()
                    {
                        ImageUrl = await photo.CreateFileAsync(_env.WebRootPath, _folder),
                        CarId = existed.Id,
                        IsMain = false,
                        CreatedAt = DateTime.Now,
                    };
                    existed.CarImages.Add(additional);
                }
            }
            if (carVM.FeatureIds != null)
            {
                List<int> createList = carVM.FeatureIds.Where(f => !existed.CarFeatures.Exists(cf => cf.FeatureId == f)).ToList();
                foreach (int featureId in createList)
                {
                    if (!(await _context.Features.AnyAsync(f => f.Id == featureId)))
                    {
                        ViewBag.Categories = _context.Categories.AsEnumerable();
                        ViewBag.BodyTypes = _context.BodyTypes.AsEnumerable();
                        ViewBag.FuelTypes = _context.FuelTypes.AsEnumerable();
                        ViewBag.Markas = _context.Markas.AsEnumerable();
                        ViewBag.Features = _context.Features.AsEnumerable();
                        ViewBag.Colors = _context.Colors.AsEnumerable();
                        carVM = carVM.MapImages(existed);
                        ModelState.AddModelError("FeatureIds", $"There is no feature has {featureId} id or ot was deleted");
                        return View(carVM);
                    }
                    CarFeature carFeature = new CarFeature()
                    {
                        FeatureId = featureId,
                        CarId = existed.Id,
                        UpdatedAt = DateTime.Now
                    };
                    existed.CarFeatures.Add(carFeature);
                }
                List<CarFeature> removeList = existed.CarFeatures.Where(cf => !carVM.FeatureIds.Contains(cf.FeatureId)).ToList();
                if (removeList != null)
                {
                    _context.CarFeatures.RemoveRange(removeList);
                }
            }
            else
            {
                List<CarFeature> removed = existed.CarFeatures.ToList();
                _context.CarFeatures.RemoveRange(removed);
            }

            existed.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Car car = await _context.Cars.Where(c => c.Id == id)
                //.Include(c => c.Marka)
                .Include(c => c.BodyType)
                .Include(c => c.FuelType)
                .Include(c => c.Category)
                .Include(c => c.CarColors).ThenInclude(cc => cc.Color)
                .Include(c => c.CarFeatures).ThenInclude(cf => cf.Feature)
                .Include(c => c.CarImages)
                .FirstOrDefaultAsync();
            if (car is null) throw new NotFoundException("There is no car has this id or it was deleted");
            IQueryable<CarImages> carImages = _context.CarImages.Where(ci => ci.CarId == id);
            foreach(CarImages carImage in carImages)
            {
                carImage.ImageUrl.DeleteFile(_env.WebRootPath, _folder);
            }
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Car car = await _context.Cars.Where(c => c.Id == id)
                .Include(c => c.Model).ThenInclude(m=>m.Marka)
                .Include(c => c.BodyType)
                .Include(c => c.FuelType)
                .Include(c => c.Category)
                .Include(c => c.CarColors).ThenInclude(cc => cc.Color)
                .Include(c => c.CarFeatures).ThenInclude(cf => cf.Feature)
                .Include(c => c.CarImages)
                .FirstOrDefaultAsync();
            if (car is null) throw new NotFoundException("There is no car has this id or it was deleted");
            return View(car);
        }
        [HttpGet]
        public IActionResult GetModelsByMarka(int markaId)
        {
            // Retrieve the models based on the markaId
            var models = _carRepository.GetModelsByMarka(markaId);

            // Return the models as JSON
            return Json(models);
        }
    }
}
