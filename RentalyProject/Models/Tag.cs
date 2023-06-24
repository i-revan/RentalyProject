using RentalyProject.Models.Base;

namespace RentalyProject.Models
{
    public class Tag:BaseHasName
    {
        public ICollection<NewsTag>? NewsTags { get; set; }
    }
}
