using FluentValidation;
using RentalyProject.ViewModels.Comments;
using RentalyProject.Utilities.Extensions;
namespace RentalyProject.Validators
{
    public class CommentValidator : AbstractValidator<CommentVM>
    {
        public CommentValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please enter your name")
                .NotNull().WithMessage("Please enter your name")
                .MinimumLength(3).WithMessage("Name cannot be shorter than 3")
                .Must(name => name.CheckIdentity()).WithMessage("Enter your name correctly");
            RuleFor(c => c.Surname)
               .NotEmpty().WithMessage("Please enter your surname")
               .NotNull().WithMessage("Please enter your surname")
               .MinimumLength(3).WithMessage("Surname cannot be shorter than 3")
               .Must(surname => surname.CheckIdentity()).WithMessage("Enter your surname correctly");
            RuleFor(c => c.Email)
                .EmailAddress().WithMessage("Enter your email correctly!")
                .NotEmpty().WithMessage("Email cannot be empty!")
                .Must(email => email.CheckMail()).WithMessage("Email format is not correct.");
            RuleFor(c => c.Message)
                .NotEmpty().WithMessage("Please enter your comment")
                .NotNull().WithMessage("Please enter your comment");
        }
    }
}
