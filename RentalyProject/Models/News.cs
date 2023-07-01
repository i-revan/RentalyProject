using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class News : BaseEntity
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public List<NewsTag>? NewsTags { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }

}
