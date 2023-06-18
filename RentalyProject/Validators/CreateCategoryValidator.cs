using FluentValidation;
using RentalyProject.ViewModels.Categories;

namespace RentalyProject.Validators
{
    public class CreateCategoryValidator:AbstractValidator<CreateCategoryVM>
    {
        public CreateCategoryValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull().WithMessage("Name is required.");
            RuleFor(c => c.Photo)
                .NotEmpty()
                .NotNull().WithMessage("Photo is required.");
        }
    }
}
