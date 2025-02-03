using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class BlogPostUpdateDTO
    {

        public string Text { get; set; }
        public string AuthorId { get; set; }

        public string UserName { get; set; }
    }
}
