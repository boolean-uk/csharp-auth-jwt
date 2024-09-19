using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataViews
{
    [NotMapped]
    public class BlogPostView
    {
        public string Text { get; set; }
    }
}
