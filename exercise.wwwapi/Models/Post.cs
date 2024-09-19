using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models
{
    [Table("post")]
    public class Post
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("author_id")]
        [ForeignKey("User")]
        public int AuthorId { get; set; }

      
        [JsonIgnore]
        public List<Comment> Comments { get; set; }

        [JsonIgnore]
        public User Author { get; set; }

    }
}
