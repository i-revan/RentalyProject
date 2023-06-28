using RentalyProject.Models;

namespace RentalyProject.ViewModels
{
    public class CarsVM
    {
        public IEnumerable<Car> Cars { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<BodyType> BodyTypes { get; set; }
    }
}
