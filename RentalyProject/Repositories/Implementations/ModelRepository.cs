using RentalyProject.DAL;
using RentalyProject.Models;
using RentalyProject.Repositories.Implementations.Generic;
using RentalyProject.Repositories.Interfaces;

namespace RentalyProject.Repositories.Implementations
{
    public class ModelRepository:Repository<Model>,IModelRepository
    {
        private readonly AppDbContext context;

        public ModelRepository(AppDbContext context):base(context)
        {

        }
    }
}
