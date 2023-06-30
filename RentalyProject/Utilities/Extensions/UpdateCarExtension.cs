using RentalyProject.Models;
using RentalyProject.ViewModels.Cars;

namespace RentalyProject.Utilities.Extensions
{
    public static class UpdateCarExtension
    {
        public static UpdateCarVM MapImages(this UpdateCarVM carVM,Car car)
        {
            carVM.CarImageVMs = new List<CarImageVM>();
            foreach (CarImages image in car.CarImages)
            {
                CarImageVM imageVM = new CarImageVM()
                {
                    Id = image.Id,
                    ImageUrl = image.ImageUrl,
                    IsMain = image.IsMain
                };
                carVM.CarImageVMs.Add(imageVM);
            }
            return carVM;
        }
    }
}
