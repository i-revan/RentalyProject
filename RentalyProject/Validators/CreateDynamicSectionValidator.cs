using FluentValidation;
using RentalyProject.ViewModels.DynamicSections;

namespace RentalyProject.Validators
{
    public class CreateDynamicSectionValidator:AbstractValidator<CreateDynamicSectionVM>
    {
        public CreateDynamicSectionValidator()
        {
            RuleFor(ds=>ds.Title)
                .NotNull().WithMessage("Title can not be empty")
                .NotEmpty().WithMessage("Title can not be empty");
            RuleFor(ds => ds.Description)
                .NotNull().WithMessage("Description can not be empty")
                .NotEmpty().WithMessage("Description can not be empty");
            RuleFor(ds=>ds.HoursOfWorks)
                .NotNull().WithMessage("Enter hours of works")
                .NotEmpty().WithMessage("Enter hours of works")
                .Must(hours=>hours>0).WithMessage("Enter hours of works correctly!");
            RuleFor(ds => ds.Awards)
                .NotNull().WithMessage("Enter number of awards")
                .NotEmpty().WithMessage("Enter number of awards")
                .Must(awards => awards > 0).WithMessage("Enter numbers of awards correctly!");
            RuleFor(ds => ds.Clients)
                .NotNull().WithMessage("Enter the number of clients")
                .NotEmpty().WithMessage("Enter the number of clients")
                .Must(clients => clients > 0).WithMessage("Enter numbers of clients correctly!");
            RuleFor(ds => ds.ExperienceYears)
                .NotNull().WithMessage("Enter the experience years")
                .NotEmpty().WithMessage("Enter the experience years")
                .Must(years => years > 0).WithMessage("Enter experience years correctly!");
            RuleFor(ds => ds.Photo)
                .NotNull().WithMessage("Add photo")
                .NotEmpty().WithMessage("Add photo");
        }
    }
}
