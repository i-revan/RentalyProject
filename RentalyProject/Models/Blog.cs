using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Blog:BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
