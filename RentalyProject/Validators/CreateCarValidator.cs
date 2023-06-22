using FluentValidation;
using RentalyProject.ViewModels.Cars;

namespace RentalyProject.Validators
{
    public class CreateCarValidator:AbstractValidator<CreateCarVM>
    {
        public CreateCarValidator()
        {
            RuleFor(c => c.Seats)
                .NotEmpty().WithMessage("Enter the number of seats")
                .NotNull().WithMessage("Enter the number of seats");
            RuleFor(c=>c.Doors)
                .NotEmpty().WithMessage("Enter the number of doors")
                .NotNull().WithMessage("Enter the number of doors");
            RuleFor(c => c.Luggage)
                .NotEmpty().WithMessage("Enter luggage")
                .NotNull().WithMessage("Enter luggage");
            RuleFor(c => c.EngineCapacity)
                .NotEmpty().WithMessage("Enter the engine capacity")
                .NotNull().WithMessage("Enter the engine capacity");
            RuleFor(c => c.Year)
                .NotEmpty().WithMessage("Enter year of the car")
                .NotNull().WithMessage("Enter year of the car");
            RuleFor(c => c.Milleage)
                .NotEmpty().WithMessage("Enter the milleage of the car")
                .NotNull().WithMessage("Enter the milleage of the car");
            RuleFor(c => c.Transmission)
                .NotEmpty().WithMessage("Enter the transmission of the car")
                .NotNull().WithMessage("Enter the transmission of the car");
            RuleFor(c => c.FuelEconomy)
                .NotEmpty().WithMessage("Enter the fuel economy of the car")
                .NotNull().WithMessage("Enter the fuel economy of the car");
            RuleFor(c => c.RentPrice)
                .NotEmpty().WithMessage("Enter the rent price for the car")
                .NotNull().WithMessage("Enter the rent price for the car");
            RuleFor(c => c.BodyTypeId)
                .NotEmpty().WithMessage("Select body type of the car");
            RuleFor(c => c.FuelTypeId)
                .NotEmpty().WithMessage("Select fuel type of the car");
            RuleFor(c => c.MarkaId)
                .NotEmpty().WithMessage("Select marka of the car");
            RuleFor(c => c.CategoryId)
                .NotEmpty().WithMessage("Select category of the car");
            RuleFor(c => c.MainPhoto)
                .NotEmpty().WithMessage("Select a photo for the car");
            RuleFor(c => c.ColorIds)
                .NotEmpty().WithMessage("Select interior and exterior color of the car");
        }
    }
}
