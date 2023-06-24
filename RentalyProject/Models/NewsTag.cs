using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class NewsTag:BaseEntity
    {
        public int NewsId { get; set; }
        public News News { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
