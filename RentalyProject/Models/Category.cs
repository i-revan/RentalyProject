using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Category:BaseHasName
    {
        public ICollection<Car> Cars { get; set; }
    }
}
