using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class DynamicSection:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int HoursOfWorks { get; set; }
        public int Clients { get; set; }
        public int Awards { get; set; }
        public int ExperienceYears { get; set; }
        public string ImageUrl { get; set; }
    }
}
