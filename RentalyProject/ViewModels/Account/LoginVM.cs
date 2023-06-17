using System.ComponentModel.DataAnnotations;

namespace RentalyProject.ViewModels.Account
{
    public class LoginVM
    {
        public string? UsernameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
