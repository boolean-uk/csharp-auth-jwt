using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Models
{
    public class BlogPost
    {

        public int Id { get; set; }

        public string text { get; set; }

        public string authorId {  get; set; }

        public Author author { get; set; }

        public ICollection<Comment> comments { get; set; }
    }
}
