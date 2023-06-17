using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class FuelType:BaseHasName
    {
        public ICollection<Car> Cars { get; set; }
    }
}
