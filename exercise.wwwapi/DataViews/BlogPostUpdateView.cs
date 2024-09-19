using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataViews
{
    [NotMapped]
    public class BlogPostUpdateView
    {
        public string Text { get; set; }
        public int AuthorId { get; set; }
    }
}
