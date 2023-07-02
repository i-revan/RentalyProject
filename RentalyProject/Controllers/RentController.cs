using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Interfaces;
using RentalyProject.Models;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.ViewModels;
using Stripe;
using System.Security.Claims;

namespace RentalyProject.Controllers
{
    [Authorize]
    public class RentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public RentController(AppDbContext context, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Car car = await _context.Cars
                .Include(c => c.Transmission)
                .Include(c => c.Model).ThenInclude(m => m.Marka)
                .Include(c => c.FuelType)
                .Include(c => c.BodyType)
                .Include(c => c.CarImages)
                .Include(c => c.CarFeatures).ThenInclude(cf => cf.Feature)
                .Include(c => c.CarColors).ThenInclude(cc => cc.Color)
                .FirstOrDefaultAsync(c => c.Id == id);

            ReservationVM reservationVM = new ReservationVM()
            {
                Car = car
            };

            return View(reservationVM);
        }
        [HttpPost]
        public async Task<IActionResult> Details(int? id, ReservationVM reservationVM, string stripeEmail, string stripeToken)
        {
            if (id is null || id < 1) throw new BadRequestException("Id is not found");
            Car car = await _context.Cars
                .Include(c => c.Model).ThenInclude(m => m.Marka)
                .Include(c => c.FuelType)
                .Include(c => c.BodyType)
                .Include(c => c.CarImages)
                .Include(c => c.CarFeatures).ThenInclude(cf => cf.Feature)
                .Include(c => c.CarColors).ThenInclude(cc => cc.Color)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (car is null) throw new NotFoundException("There is no car has this id or it was deleted");
            reservationVM.Car = car;
            if ((reservationVM.ReturnDate - reservationVM.PickUpDate).Days > 7)
            {
                ModelState.AddModelError("ReturnDate", "Maximum reservation duration is 7 days");
                return View(reservationVM);
            }
            if (!ModelState.IsValid)
            {
                return View(reservationVM);
            }

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null) throw new NotFoundException("User is not found");
            Reservation reservation = new Reservation()
            {
                PickUpLocation = reservationVM.PickUpLocation,
                DropOffLocation = reservationVM.DropOffLocation,
                PickUpDate = reservationVM.PickUpDate,
                ReturnDate = reservationVM.ReturnDate,
                AppUserId = user.Id,
                Status = null,
                CarId = car.Id,
                CreatedAt = DateTime.Now
            };



            //Stripe
            var optionCust = new CustomerCreateOptions
            {
                Email = stripeEmail,
                Name = user.Name + " " + user.Surname,
                Phone = user.PhoneNumber
            };
            var serviceCust = new CustomerService();
            Customer customer = serviceCust.Create(optionCust);

            var total = car.RentPrice * (reservation.ReturnDate - reservation.PickUpDate).Days;
            total *= 100;
            var optionsCharge = new ChargeCreateOptions
            {

                Amount = (long)total,
                Currency = "USD",
                Description = "Car Renting Amount",
                Source = stripeToken,
                ReceiptEmail = stripeEmail


            };
            var serviceCharge = new ChargeService();
            Charge charge = serviceCharge.Create(optionsCharge);

            if (charge.Status != "succeeded")
            {
                ModelState.AddModelError(String.Empty, "There occur some problem");
                return View();
            }
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            string body = @"<table>
                                <thead>
                                    <tr>
                                        <th>Car</th>
                                        <th>Payment</th>
                                        <th>Reservation Date</th>

                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
            
                                    ";

            body += @$"
                    <td>{reservation.Car.Model.Marka.Name} {reservation.Car.Model.Name}</td>
                    <td>${String.Format("{0:#,##0}", (reservation.ReturnDate - reservation.PickUpDate).Days * reservation.Car.RentPrice)}</td>
                    <td>{reservation.CreatedAt.ToString("dd MMMM, yyyy")}</td>
                </tr>
            </tbody>
                            </table>";
            await _emailService.SendMail(user.Email, "Reservation Replacement", body, true);
            return RedirectToAction("Orders", "Home");
        }
    }
}
