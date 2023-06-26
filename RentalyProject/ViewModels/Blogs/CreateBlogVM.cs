namespace RentalyProject.ViewModels.Blogs
{
    public class CreateBlogVM
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
