using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Feature:BaseHasName
    {
        public ICollection<CarFeature> CarFeatures { get; set; }
    }
}
