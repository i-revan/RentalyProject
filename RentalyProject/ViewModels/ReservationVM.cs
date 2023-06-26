using RentalyProject.Models;
using System.ComponentModel.DataAnnotations;

namespace RentalyProject.ViewModels
{
    public class ReservationVM
    {
        public string PickUpLocation { get; set; }
        public string DropOffLocation { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public Car? Car { get; set; }
    }
}
