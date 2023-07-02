using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Transmission:BaseHasName
    {
        public ICollection<Car> Cars { get; set; }
    }
}
