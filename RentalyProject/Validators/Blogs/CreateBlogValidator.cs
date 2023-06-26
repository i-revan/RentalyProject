using FluentValidation;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.Blogs;

namespace RentalyProject.Validators.Blogs
{
    public class CreateBlogValidator:AbstractValidator<CreateBlogVM>
    {
        public CreateBlogValidator()
        {
            RuleFor(cb => cb.Name)
                .NotEmpty().WithMessage("Enter your name")
                .NotNull().WithMessage("Enter your name")
                .Must(name=>name.CheckIdentity()).WithMessage("Enter your name correctly");

            RuleFor(cb => cb.Surname)
                .NotEmpty().WithMessage("Enter your surname")
                .NotNull().WithMessage("Enter your surname")
                .Must(name => name.CheckIdentity()).WithMessage("Enter your surname correctly");
            RuleFor(cb => cb.Title)
                .NotEmpty().WithMessage("Enter title")
                .NotNull().WithMessage("Enter title");
            RuleFor(cb => cb.Description)
                .NotEmpty().WithMessage("Enter description")
                .NotNull().WithMessage("Enter description");
            RuleFor(cb => cb.Photo)
                .NotEmpty().WithMessage("Add your photo")
                .NotNull().WithMessage("Add your photo");
        }
    }
}
