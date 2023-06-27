using FluentValidation;
using RentalyProject.ViewModels;

namespace RentalyProject.Validators
{
    public class ReservationValidator:AbstractValidator<ReservationVM>
    {
        public ReservationValidator()
        {
            RuleFor(r=>r.PickUpLocation)
                .NotNull().WithMessage("Enter pickup location")
                .NotEmpty().WithMessage("Enter pickup location");
            RuleFor(r => r.DropOffLocation)
                .NotNull().WithMessage("Enter drop off location")
                .NotEmpty().WithMessage("Enter drop off location");
            RuleFor(r => r.PickUpDate)
                .NotNull().WithMessage("Enter pickup date")
                .NotEmpty().WithMessage("Enter pickup date")
                .Must(date=>date>=DateTime.Now).WithMessage("This date is invalid");
            RuleFor(r => r.ReturnDate)
                .NotNull().WithMessage("Enter return date")
                .NotEmpty().WithMessage("Enter return date")
                .Must(date => date >= DateTime.Now).WithMessage("This date is invalid")
                .Must((reservation, returnDate) => returnDate > reservation.PickUpDate).WithMessage("Return date must be greater than the pickup date");


        }
    }
}
