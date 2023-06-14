using FluentValidation;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.Account;

namespace RentalyProject.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterVM>
    {
        public RegisterValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .NotNull().WithMessage("Please enter your name")
                .MinimumLength(3).WithMessage("Name cannot be shorter than 3")
                .Must(name=> name.CheckIdentity()).WithMessage("Enter your name correctly");
            RuleFor(r => r.Surname)
                .NotNull()
                .NotEmpty().WithMessage("Please enter your surname")
                .MinimumLength(3).WithMessage("Surname cannot be shorter than 3")
                .Must(name => name.CheckIdentity()).WithMessage("Enter your surname correctly");
            RuleFor(r => r.Username)
                .NotEmpty()
                .NotNull();
            RuleFor(r => r.Email)
                .EmailAddress().WithMessage("Enter your email correctly!")
                .NotEmpty().WithMessage("Email cannot be empty!")
                .Must(email => email.CheckMail()).WithMessage("Email format is not correct.");
            RuleFor(r => r.PhoneNumber)
                .NotNull().WithMessage("Phone number is required.")
                .NotEmpty().WithMessage("Phone number is required.")
                .Must(phone => phone.CheckPhoneNumber()).WithMessage("Phone number format is not correct.");
            RuleFor(r => r.Password)
                .NotNull()
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.");
            RuleFor(r => r.ConfirmPassword)
                .NotEmpty()
                .NotEmpty()
                .Equal(r => r.Password)
                .WithMessage("Confirm Password must match Password.");
        }
    }
}
