using System.ComponentModel.DataAnnotations;

namespace RentalyProject.ViewModels.Account
{
    public class RegisterVM
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

    }
}
