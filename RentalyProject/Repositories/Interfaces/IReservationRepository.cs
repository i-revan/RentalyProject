using RentalyProject.Models;
using RentalyProject.Repositories.Interfaces.Generic;

namespace RentalyProject.Repositories.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetExpiredReservationsAsync(DateTime currentDate);
    }
}
