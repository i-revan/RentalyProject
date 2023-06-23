using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Reservation:BaseEntity
    {
        public string PickUpLocation { get; set; }
        public string DropOffLocation { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime ReturnDate { get; set;}
        public bool? Status { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}
