using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Comment:BaseEntity
    {
        public string Description { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
