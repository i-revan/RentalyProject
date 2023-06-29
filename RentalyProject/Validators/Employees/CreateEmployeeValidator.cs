using FluentValidation;
using RentalyProject.ViewModels.Employees;
using RentalyProject.Utilities.Extensions;

namespace RentalyProject.Validators.Employees
{
    public class CreateEmployeeValidator:AbstractValidator<CreateEmployeeVM>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Please enter your name")
                .NotNull().WithMessage("Please enter your name")
                .MinimumLength(3).WithMessage("Name length cannot be shorter than 3")
                .Must(name => name.CheckIdentity()).WithMessage("Enter your name correctly");
            RuleFor(r => r.Surname)
                .NotNull().WithMessage("Please enter your surname")
                .NotEmpty().WithMessage("Please enter your surname")
                .MinimumLength(3).WithMessage("Surname cannot be shorter than 3")
                .Must(name => name.CheckIdentity()).WithMessage("Enter your surname correctly");
            RuleFor(r => r.Photo)
                .NotNull().WithMessage("Add photo")
                .NotEmpty().WithMessage("Add photo");
            RuleFor(r => r.PositionId)
                .NotNull().WithMessage("Select position")
                .NotEmpty().WithMessage("Select position");
            RuleFor(r => r.FacebookLink)
                .NotEmpty().WithMessage("Please enter facebook link")
                .NotNull().WithMessage("Please enter facebook link");
            RuleFor(r => r.TwitterLink)
                .NotEmpty().WithMessage("Please enter twitter link")
                .NotNull().WithMessage("Please enter twitter link");
            RuleFor(r => r.LinkedinLink)
                .NotEmpty().WithMessage("Please enter linkedin link")
                .NotNull().WithMessage("Please enter linkedin link");
            RuleFor(r => r.PinterestLink)
                .NotEmpty().WithMessage("Please enter pinterest link")
                .NotNull().WithMessage("Please enter pinterest link");
        }
    }
}
