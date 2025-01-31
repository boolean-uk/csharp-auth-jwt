using System.Text.Json.Serialization;

namespace exercise.wwwapi.Model
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string AuthorUsername { get; set; }
        public string AuthorId { get; set; } // Accommodate the UUID

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
