using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Category:BaseHasName
    {
        public string ImageUrl { get; set; }
        public ICollection<Car> Cars { get; set; }
        public List<BodyTypeCategory> BodyTypeCategories { get; set; }
    }
}
