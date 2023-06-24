using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Service:BaseEntity
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
