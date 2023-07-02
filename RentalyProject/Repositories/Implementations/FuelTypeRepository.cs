using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Repositories.Implementations.Generic;
using RentalyProject.Repositories.Interfaces;

namespace RentalyProject.Repositories.Implementations
{
    public class FuelTypeRepository:Repository<FuelType>,IFuelTypeRepository
    {
        public FuelTypeRepository(AppDbContext context):base(context)
        {
            
        }
    }
}
