using FluentValidation;
using RentalyProject.ViewModels.Transmissions;

namespace RentalyProject.Validators
{
    public class TransmissionValidator:AbstractValidator<TransmissionVM>
    {
        public TransmissionValidator()
        {
            RuleFor(t => t.Name)
                .NotNull().WithMessage("Enter transmission name")
                .NotEmpty().WithMessage("Enter transmission name");
        }
    }
}
