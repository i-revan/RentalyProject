using RentalyProject.Models.Base;
using System.Diagnostics.Contracts;

namespace RentalyProject.Models
{
    public class CarFeature:BaseEntity
    {
        public int CarId { get; set; }
        public int FeatureId { get; set; }
        public Car Car { get; set; }
        public Feature Feature { get; set; }
    }
}
