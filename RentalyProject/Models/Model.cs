using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Model:BaseHasName
    {
        public int MarkaId { get; set; }
        public Marka Marka { get; set; }
    }
}
