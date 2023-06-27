using RentalyProject.Models;

namespace RentalyProject.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<BodyType> BodyTypes { get; set; }
        public IEnumerable<Car> Cars { get; set; }
        public IEnumerable<Service> Services { get; set; }
        public IEnumerable<Faq> Faqs { get; set; }
        public IEnumerable<News> News { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }
        public IEnumerable<FavoriteCar>? UserFavorites { get; set; }
    }
}
