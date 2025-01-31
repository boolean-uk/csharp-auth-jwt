using System.Text.Json.Serialization;

namespace exercise.wwwapi.Model
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BlogPostId { get; set; }

        [JsonIgnore]
        public BlogPost BlogPost { get; set; }
    }
}
