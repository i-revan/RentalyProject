using RentalyProject.Models;

namespace RentalyProject.ViewModels.Cars
{
    public class CreateCarVM
    {
        public string Description { get; set; }
        public int Like { get; set; }
        public int Seats { get; set; }
        public int Doors { get; set; }
        public int Luggage { get; set; }
        public int EngineCapacity { get; set; }
        public int Year { get; set; }
        public int Milleage { get; set; }
        public string? Transmission { get; set; }
        public decimal FuelEconomy { get; set; }
        public decimal RentPrice { get; set; }
        public int BodyTypeId { get; set; }
        public int FuelTypeId { get; set; }
        public int ModelId { get; set; }
        public int MarkaId { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? MainPhoto { get; set; }
        public ICollection<IFormFile>? Photos { get; set; }
        public ICollection<int>? FeatureIds { get; set; }
        public List<int>? ColorIds { get; set; }
    }
}
