using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Employee:BaseHasName
    {
        public string Surname { get; set; }
        public string ImageUrl { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string LinkedinLink { get; set; }
        public string PinterestLink { get; set; }
    }
}
