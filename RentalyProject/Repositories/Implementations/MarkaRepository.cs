using NuGet.Protocol.Core.Types;
using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Repositories.Implementations.Generic;
using RentalyProject.Repositories.Interfaces;

namespace RentalyProject.Repositories.Implementations
{
    public class MarkaRepository:Repository<Marka>,IMarkaRepository
    {
        public MarkaRepository(AppDbContext context):base(context)
        {
            
        }
    }
}
