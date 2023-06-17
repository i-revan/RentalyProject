using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class BodyTypeCategory:BaseEntity
    {
        public int BodyTypeId { get; set; }
        public BodyType BodyType { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
