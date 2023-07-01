using RentalyProject.Models;
using RentalyProject.Repositories.Interfaces;
using RentalyProject.Utilities.Exceptions;

namespace RentalyProject.Services
{
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICarRepository _carRepository;

        public ReservationService(IReservationRepository reservationRepository, ICarRepository carRepository)
        {
            _reservationRepository = reservationRepository;
            _carRepository = carRepository;
        }

        public async Task CompleteExpiredReservations()
        {
            DateTime currentDate = DateTime.Now;

            // Get expired reservations
            IEnumerable<Reservation> expiredReservations = await _reservationRepository.GetExpiredReservationsAsync(currentDate);

            foreach (Reservation reservation in expiredReservations)
            {
                // Update the car availability
                Car car = await _carRepository.GetByIdAsync(reservation.CarId);
                if (car != null)
                {
                    car.IsAvailable = true;
                    await _carRepository.UpdateAsync(car);
                }
            }
        }
    }
}
