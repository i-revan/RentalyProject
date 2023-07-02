using Microsoft.EntityFrameworkCore;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Repositories.Implementations.Generic;
using RentalyProject.Repositories.Interfaces;

namespace RentalyProject.Repositories.Implementations
{
    public class CarRepository:Repository<Car>,ICarRepository
    {
        public CarRepository(AppDbContext context):base(context)
        {
            
        }
        public IEnumerable<Model> GetModelsByMarka(int markaId)
        {
            return _context.Models.Where(m => m.MarkaId == markaId).ToList();
        }
    }
}
