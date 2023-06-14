using System.ComponentModel.DataAnnotations;

namespace RentalyProject.ViewModels.Account
{
    public class LoginVM
    {
        [Required]
        public string UsernameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
