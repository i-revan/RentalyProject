using RentalyProject.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace RentalyProject.Models
{
    public class Comment:BaseHasName
    {
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public int NewsId { get; set; }
        public News News { get; set; }
    }
}
