using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class BodyType:BaseHasName
    {
        public ICollection<Car> Cars { get; set; }
        public ICollection<BodyTypeCategory> BodyTypeCategories { get; set; }
    }
}
