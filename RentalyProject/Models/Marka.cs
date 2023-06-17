using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Marka:BaseHasName
    {
        public ICollection<Car> Cars { get; set; }
        public ICollection<Model> Models { get; set; }
    }
}
