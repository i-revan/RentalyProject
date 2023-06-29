using FluentValidation;
using RentalyProject.ViewModels;

namespace RentalyProject.Validators
{
    public class ResetPasswordValidator:AbstractValidator<ResetPasswordVM>
    {
        public ResetPasswordValidator()
        {
            RuleFor(r => r.Password)
                .NotNull().WithMessage("Password is required.")
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.");
            RuleFor(r => r.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm your password")
                .NotEmpty().WithMessage("Confirm your password")
                .Equal(r => r.Password)
                .WithMessage("Confirm Password must match Password.");
        }
    }
}
