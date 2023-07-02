using Microsoft.EntityFrameworkCore;
using RentalyProject.Models;
using RentalyProject.Repositories.Interfaces.Generic;

namespace RentalyProject.Repositories.Interfaces
{
    public interface ICarRepository : IRepository<Car>
    {
        IEnumerable<Model> GetModelsByMarka(int markaId);
    }
}
