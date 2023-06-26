using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class UserQuestion:BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
    }
}
