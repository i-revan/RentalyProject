using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Repositories.Implementations.Generic;
using RentalyProject.Repositories.Interfaces;

namespace RentalyProject.Repositories.Implementations
{
    public class ReservationRepository:Repository<Reservation>,IReservationRepository
    {
        public ReservationRepository(AppDbContext context):base(context)
        {
            
        }

        public async Task<IEnumerable<Reservation>> GetExpiredReservationsAsync(DateTime currentDate)
        {
            return await _dbSet.Where(reservation => reservation.ReturnDate < currentDate).ToListAsync();
        }
    }
}
