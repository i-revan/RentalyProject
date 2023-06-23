using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class FavoriteCar:BaseEntity
    {
        public int CarId { get; set; }
        public Car Car { get; set; }
        public decimal RentPrice { get; set; }
        public string AppUserid { get; set; }
        public AppUser AppUser { get; set; }
    }
}
