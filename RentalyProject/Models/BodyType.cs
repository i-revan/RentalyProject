using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class BodyType:BaseHasName
    {
        public ICollection<Car> Cars { get; set; }
    }
}
