using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.ViewModels;
using System.Net;

namespace RentalyProject.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public FavoriteController(AppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<FavoriteItemVM> favoriteCars = new List<FavoriteItemVM>();
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user is null) throw new NotFoundException("User is not found");
                List<FavoriteCar> userFavorites = await _context.FavoriteCars.Where(fc=>fc.AppUserid==user.Id)
                    .Include(fc=>fc.Car)
                        .ThenInclude(c=>c.CarImages.Where(ci=>ci.IsMain==true))
                    .Include(fc=>fc.Car.Marka)
                    .Include(fc=>fc.Car.FuelType)
                    .Include(fc=>fc.Car.BodyType)
                    .ToListAsync();
                
                foreach(FavoriteCar item in userFavorites)
                {
                    favoriteCars.Add(new FavoriteItemVM
                    {
                        Id = item.Car.Id,
                        RentPrice = item.RentPrice,
                        Doors = item.Car.Doors,
                        Seats = item.Car.Seats,
                        Luggage = item.Car.Luggage,
                        FuelType = item.Car.FuelType.Name,
                        BodyType = item.Car.BodyType.Name,
                        Marka = item.Car.Marka.Name,
                        EngineCapacity = item.Car.EngineCapacity,
                        ImageUrl = item.Car.CarImages.FirstOrDefault().ImageUrl

                    });

                }
            }
            else
            {
                List<FavoriteCookiesVM> favorites;
                string json = Request.Cookies["Favorite"];
                if (!String.IsNullOrEmpty(json))
                {
                    favorites = JsonConvert.DeserializeObject<List<FavoriteCookiesVM>>(Request.Cookies["Favorite"]);
                }
                else
                {
                    favorites = new List<FavoriteCookiesVM>();
                }
                for (int i = 0; i < favorites.Count; i++)
                {
                    Car car = await _context.Cars.Include(c => c.Marka)
                    .Include(c => c.FuelType)
                    .Include(c => c.BodyType)
                    .Include(c => c.CarImages.Where(ci => ci.IsMain == true)).FirstOrDefaultAsync(c => c.Id == favorites[i].Id);
                    if (car is null)
                    {
                        favorites.Remove(favorites[i]);
                        continue;
                    }
                    FavoriteItemVM favoriteItemVM = new FavoriteItemVM()
                    {
                        Id = car.Id,
                        BodyType = car.BodyType.Name,
                        EngineCapacity = car.EngineCapacity,
                        FuelType = car.FuelType.Name,
                        Doors = car.Doors,
                        Marka = car.Marka.Name,
                        RentPrice = car.RentPrice,
                        Seats = car.Seats,
                        Luggage = car.Luggage,
                        ImageUrl = car.CarImages.FirstOrDefault().ImageUrl
                    };
                    favoriteCars.Add(favoriteItemVM);
                }
                //foreach (var cookie in favorites)
                //{
                //    Car car = await _context.Cars.Include(c => c.Marka)
                //    .Include(c => c.FuelType)
                //    .Include(c => c.BodyType)
                //    .Include(c => c.CarImages.Where(ci=>ci.IsMain==true)).FirstOrDefaultAsync(c => c.Id == cookie.Id);
                //    if(car is null)
                //    {
                //        favorites.Remove(cookie);
                //        continue;
                //    }
                //    FavoriteItemVM favoriteItemVM = new FavoriteItemVM()
                //    {
                //        Id = car.Id,
                //        BodyType = car.BodyType.Name,
                //        EngineCapacity = car.EngineCapacity,
                //        FuelType = car.FuelType.Name,
                //        Doors = car.Doors,
                //        Marka = car.Marka.Name,
                //        RentPrice = car.RentPrice,
                //        Seats = car.Seats,
                //        Luggage = car.Luggage,
                //        ImageUrl = car.CarImages.FirstOrDefault().ImageUrl
                //    };
                //    favoriteCars.Add(favoriteItemVM);

                //}
            }

            return View(favoriteCars);
        }
        public async Task<IActionResult> AddFavorite(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Car car = await _context.Cars.Where(c => c.Id == id)
                .Include(c => c.CarImages).FirstOrDefaultAsync();
            if (car is null) throw new NotFoundException("There is no car has this id or it was deleted");
            if (User.Identity.IsAuthenticated)
            {
                car.Like += 1;
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user is null) throw new NotFoundException("User is not found");
                
                bool result = await _context.FavoriteCars.AnyAsync(f=>f.CarId == car.Id && f.AppUserid == user.Id);
                if (!result)
                {
                    FavoriteCar favoriteCar = new FavoriteCar()
                    {
                        AppUserid = user.Id,
                        CarId = car.Id,
                        CreatedAt = DateTime.Now,
                        RentPrice = car.RentPrice
                    };
                    await _context.FavoriteCars.AddAsync(favoriteCar);
                }
                await _context.SaveChangesAsync();

            }
            else
            {
                List<FavoriteCookiesVM> favorites;
                if (Request.Cookies["Favorite"] is null)
                {
                    favorites = new List<FavoriteCookiesVM>();
                    favorites.Add(new FavoriteCookiesVM
                    {
                        Id = car.Id
                    });
                }
                else
                {
                    favorites = JsonConvert.DeserializeObject<List<FavoriteCookiesVM>>(Request.Cookies["Favorite"]);
                    FavoriteCookiesVM existed = favorites.FirstOrDefault(f => f.Id == id);
                    if (existed != null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        favorites.Add(new FavoriteCookiesVM
                        {
                            Id = car.Id
                        });
                    }

                }

                string json = JsonConvert.SerializeObject(favorites);
                Response.Cookies.Append("Favorite", json);
            }
            
            
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> GetFavorite()
        {
            var favorite = JsonConvert.DeserializeObject<List<FavoriteCookiesVM>>(Request.Cookies["Favorite"]);
            return Json(favorite);
        }
    }
}
