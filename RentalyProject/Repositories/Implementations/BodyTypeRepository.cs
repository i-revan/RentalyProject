using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Repositories.Implementations.Generic;
using RentalyProject.Repositories.Interfaces;

namespace RentalyProject.Repositories.Implementations
{
    public class BodyTypeRepository:Repository<BodyType>,IBodyTypeRepository
    {
        public BodyTypeRepository(AppDbContext context):base(context)
        {
            
        }
    }
}
