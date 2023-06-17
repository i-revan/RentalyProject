﻿namespace RentalyProject.ViewModels.Category
{
    public class CreateCategoryVM
    {
        public string Name { get; set; }
        public IFormFile Photo { get; set; }
        public ICollection<int>? BodyTypeIds { get; set; }
    }
}
