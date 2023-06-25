using FluentValidation;
using RentalyProject.ViewModels.Faqs;

namespace RentalyProject.Validators
{
    public class FaqValidator:AbstractValidator<FaqVM>
    {
        public FaqValidator()
        {
            RuleFor(f => f.Question)
                .NotEmpty().WithMessage("Question can not be empty")
                .NotNull().WithMessage("Question can not be empty");
            RuleFor(f => f.Answer)
                .NotEmpty().WithMessage("Answer can not be empty")
                .NotNull().WithMessage("Answer can not be empty");
        }
    }
}
