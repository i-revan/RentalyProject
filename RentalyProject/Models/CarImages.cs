using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class CarImages:BaseEntity
    {
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}
