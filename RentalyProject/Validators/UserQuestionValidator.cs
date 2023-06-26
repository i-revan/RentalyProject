using FluentValidation;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.UserQuestions;

namespace RentalyProject.Validators
{
    public class UserQuestionValidator:AbstractValidator<UserQuestionVM>
    {
        public UserQuestionValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Please enter your name")
                .NotNull().WithMessage("Please enter your name")
                .MinimumLength(3).WithMessage("Name cannot be shorter than 3")
                .Must(name => name.CheckIdentity()).WithMessage("Enter your name correctly");
            RuleFor(r => r.Email)
                .EmailAddress().WithMessage("Enter your email correctly!")
                .NotEmpty().WithMessage("Email cannot be empty!")
                .Must(email => email.CheckMail()).WithMessage("Email format is not correct.");
            RuleFor(r => r.Phone)
                .NotNull().WithMessage("Phone number is required.")
                .NotEmpty().WithMessage("Phone number is required.")
                .Must(phone => phone.CheckPhoneNumber()).WithMessage("Phone number format is not correct.");
            RuleFor(r => r.Message)
                .NotEmpty().WithMessage("You can not send empty message")
                .NotNull().WithMessage("You can not send empty message");

        }
    }
}
