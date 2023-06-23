namespace RentalyProject.ViewModels
{
    public class FavoriteItemVM
    {
        public int Id { get; set; }
        public int Seats { get; set; }
        public int Doors { get; set; }
        public int Luggage { get; set; }
        public string FuelType { get; set; }
        public string Marka { get; set; }
        public string BodyType { get; set; }
        public int EngineCapacity { get; set; }
        public decimal RentPrice { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsFavorite { get; set; }
    }
}
