using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Car:BaseHasName
    {
        public string Description { get; set; }
        public int Seats { get; set; }
        public int Doors { get; set; }
        public string Luggage { get; set; }
        public string FuelType { get; set; }
        public int EngineCapacity { get; set; }
        public int Year { get; set; }
        public int Milleage { get; set; }
        public string Transmission { get; set; }
        public float FuelEconomy { get; set; }
        public decimal RentPrice { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<CarImages> CarImages { get; set; }
        public ICollection<CarFeature> CarFeatures { get; set; }
        public int BodyTypeId { get; set; }
        public BodyType BodyType { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<CarColor> CarColors { get; set; }

    }
}
