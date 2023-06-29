using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Position:BaseHasName
    {
        public ICollection<Employee> Employees { get; set; }
    }
}
