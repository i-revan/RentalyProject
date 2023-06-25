using FluentValidation;
using RentalyProject.ViewModels.Newss;

namespace RentalyProject.Validators
{
    public class UpdateNewsValidator:AbstractValidator<UpdateNewsVM>
    {
        public UpdateNewsValidator()
        {
            RuleFor(un => un.Title)
                .NotEmpty().WithMessage("Title can not be empty!")
                .NotNull().WithMessage("Title can not be empty!");
            RuleFor(un => un.Content)
                .NotEmpty().WithMessage("Content can not be empty!")
                .NotNull().WithMessage("Content can not be empty!");
        }
    }
}
