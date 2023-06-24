namespace RentalyProject.ViewModels.News
{
    public class CreateNewsVM
    {
        public IFormFile? Photo { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public ICollection<int>? TagIds { get; set; }
    }
}
