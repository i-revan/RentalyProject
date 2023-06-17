namespace RentalyProject.ViewModels.Category
{
    public class UpdateCategoryVM
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile? Photo { get; set; }
        public List<int>? BodyTypeIds { get; set; }
    }
}
