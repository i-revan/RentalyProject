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
                .NotNull().WithMessage("Enter the number of seats")
                .Must(seats => seats >= 2 && seats <= 10).WithMessage("Enter number of seats between 2 and 10");
            RuleFor(c=>c.Doors)
                .NotEmpty().WithMessage("Enter the number of doors")
                .NotNull().WithMessage("Enter the number of doors")
                .Must(doors => doors >= 2 && doors <= 10).WithMessage("Enter number of doors between 2 and 10");
            RuleFor(c => c.Luggage)
                .NotEmpty().WithMessage("Enter luggage")
                .NotNull().WithMessage("Enter luggage")
                .Must(luggage => luggage > 0 && luggage <= 100).WithMessage("Enter luggage between 1 and 100");
            RuleFor(c => c.EngineCapacity)
                .NotEmpty().WithMessage("Enter the engine capacity")
                .NotNull().WithMessage("Enter the engine capacity")
                .Must(engine => engine > 1000 && engine <= 4000).WithMessage("Enter engine capacity 1000 and 4000");
            RuleFor(c => c.Year)
                .NotEmpty().WithMessage("Enter year of the car")
                .NotNull().WithMessage("Enter year of the car")
                .Must(year=>year <= DateTime.Now.Year && year >= 2000).WithMessage("Enter year correctly! Year must be greater than 2000");
            RuleFor(c => c.Milleage)
                .NotEmpty().WithMessage("Enter the milleage of the car")
                .NotNull().WithMessage("Enter the milleage of the car")
                .Must(milleage => milleage > 0).WithMessage("Enter milleage correctly");
            RuleFor(c => c.Transmission)
                .NotEmpty().WithMessage("Enter the transmission of the car")
                .NotNull().WithMessage("Enter the transmission of the car");
            RuleFor(c => c.FuelEconomy)
                .NotEmpty().WithMessage("Enter the fuel economy of the car")
                .NotNull().WithMessage("Enter the fuel economy of the car")
                .Must(fueleconomy => fueleconomy > 0).WithMessage("Enter fuel economy correctly");
            RuleFor(c => c.RentPrice)
                .NotEmpty().WithMessage("Enter the rent price for the car")
                .NotNull().WithMessage("Enter the rent price for the car")
                .Must(rentPrice => rentPrice >= 100 && rentPrice<=300).WithMessage("Enter rent price between 100$ and 300$");
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
