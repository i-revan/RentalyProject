using FluentValidation;
using FluentValidation.AspNetCore;
using RentalyProject.ViewModels.Services;

namespace RentalyProject.Validators
{
    public class ServiceValidator:AbstractValidator<ServiceVM>
    {
        public ServiceValidator()
        {
            RuleFor(s => s.Icon)
                .NotNull().WithMessage("Enter icon")
                .NotEmpty().WithMessage("Enter icon");
            RuleFor(s=>s.Title)
                .NotNull().WithMessage("Enter title")
                .NotEmpty().WithMessage("Enter title");
            RuleFor(s => s.Description)
                .NotNull().WithMessage("Enter description")
                .NotEmpty().WithMessage("Enter description");
        }
    }
}
