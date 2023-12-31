﻿using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Car:BaseEntity
    {
        public string Description { get; set; }
        public int Like { get; set; }
        public int Seats { get; set; }
        public int Doors { get; set; }
        public int Luggage { get; set; }
        public int EngineCapacity { get; set; }
        public int Year { get; set; }
        public int Milleage { get; set; }
        public int TransmissionId { get; set; }
        public Transmission Transmission { get; set; }
        public decimal FuelEconomy { get; set; }
        public decimal RentPrice { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<CarImages> CarImages { get; set; }
        public List<CarFeature> CarFeatures { get; set; }
        public int BodyTypeId { get; set; }
        public BodyType BodyType { get; set; }
        public int FuelTypeId { get; set; }
        public FuelType FuelType { get; set; }
        public ICollection<CarColor> CarColors { get; set; }
        public int ModelId { get; set; }
        public Model Model { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<FavoriteCar> FavoriteCars { get; set; }

    }
}
