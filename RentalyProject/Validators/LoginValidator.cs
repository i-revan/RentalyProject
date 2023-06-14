using FluentValidation;
using RentalyProject.ViewModels.Account;

namespace RentalyProject.Validators
{
    public class LoginValidator:AbstractValidator<LoginVM>
    {
        public LoginValidator()
        {
            RuleFor(l => l.UsernameOrEmail)
                .NotNull()
                    .WithMessage("Username or Email is required.")
                .NotEmpty()
                    .WithMessage("Username or Email is required.");
            RuleFor(l => l.Password)
                .NotNull()
                    .WithMessage("Password is required.")
                .NotEmpty()
                    .WithMessage("Password is required.");
        }
    }
}
