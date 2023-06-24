using FluentValidation;
using RentalyProject.ViewModels.News;

namespace RentalyProject.Validators
{
    public class CreateNewsValidator:AbstractValidator<CreateNewsVM>
    {
        public CreateNewsValidator()
        {
            RuleFor(cn => cn.Title)
                .NotNull().WithMessage("Enter the title of news")
                .NotEmpty().WithMessage("Enter the title of news");
            RuleFor(cn => cn.Content)
                .NotNull().WithMessage("Enter content of news")
                .NotEmpty().WithMessage("Enter content of news");
            RuleFor(cn=>cn.Photo)
                .NotNull().WithMessage("Add photo")
                .NotEmpty().WithMessage("Add photo");
        }
    }
}
