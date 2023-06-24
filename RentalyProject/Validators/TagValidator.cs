using FluentValidation;
using RentalyProject.ViewModels.Tags;

namespace RentalyProject.Validators
{
    public class TagValidator:AbstractValidator<TagVM>
    {
        public TagValidator()
        {
            RuleFor(t => t.Name)
                .NotNull().WithMessage("Enter the tag name")
                .NotEmpty().WithMessage("Enter the tag name");
        }
    }
}
