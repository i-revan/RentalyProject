using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Repositories.Implementations.Generic;
using RentalyProject.Repositories.Interfaces;
using RentalyProject.Repositories.Interfaces.Generic;

namespace RentalyProject.Repositories.Implementations
{
    public class TransmissionRepository:Repository<Transmission>,ITransmissionRepository
    {
        public TransmissionRepository(AppDbContext context):base(context)
        {
            
        }
    }
}
