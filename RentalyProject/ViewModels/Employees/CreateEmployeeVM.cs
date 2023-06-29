using RentalyProject.Models;

namespace RentalyProject.ViewModels.Employees
{
    public class CreateEmployeeVM
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public IFormFile Photo { get; set; }
        public int PositionId { get; set; }
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string LinkedinLink { get; set; }
        public string PinterestLink { get; set; }
    }
}
