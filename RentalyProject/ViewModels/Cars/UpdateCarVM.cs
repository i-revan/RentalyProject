using RentalyProject.Models;

namespace RentalyProject.ViewModels.Cars
{
    public class UpdateCarVM
    {
        public string? Description { get; set; }
        public int Seats { get; set; }
        public int Doors { get; set; }
        public int Luggage { get; set; }
        public int EngineCapacity { get; set; }
        public int Year { get; set; }
        public int Milleage { get; set; }
        public int TransmissionId { get; set; }
        public decimal FuelEconomy { get; set; }
        public decimal RentPrice { get; set; }
        public int BodyTypeId { get; set; }
        public int FuelTypeId { get; set; }
        public int CategoryId { get; set; }
        public ICollection<int>? FeatureIds { get; set; }
        public IFormFile? MainPhoto { get; set; }
        public ICollection<IFormFile>? Photos { get; set; }
        public List<CarImageVM>? CarImageVMs { get; set; }
        public List<int>? ImageIds { get; set; }
    }
    public class CarImageVM
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
    }
}
