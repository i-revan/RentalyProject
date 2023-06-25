using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Faq:BaseEntity
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
