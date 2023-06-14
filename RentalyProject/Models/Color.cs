using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Color:BaseHasName
    {
        public ICollection<CarColor> CarColors { get; set; }
    }
}
