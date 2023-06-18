using FluentValidation;
using RentalyProject.ViewModels.Models;

namespace RentalyProject.Validators
{
    public class ModelVMValidator:AbstractValidator<ModelVM>
    {
        public ModelVMValidator()
        {
            RuleFor(m=>m.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Name is required.");
            RuleFor(m => m.MarkaId)
                .NotEmpty()
                .NotNull()
                .WithMessage("Please select marka.");
        }
    }
}
