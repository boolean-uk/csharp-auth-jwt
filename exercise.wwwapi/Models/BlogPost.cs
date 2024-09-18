using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models
{
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        [JsonIgnore]
        public User Authour { get; set; }
    }
}
