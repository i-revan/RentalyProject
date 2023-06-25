﻿namespace RentalyProject.ViewModels.DynamicSections
{
    public class UpdateDynamicSectionVM
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int HoursOfWorks { get; set; }
        public int Clients { get; set; }
        public int Awards { get; set; }
        public int ExperienceYears { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
