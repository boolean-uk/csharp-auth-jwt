using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models
{
    [Table("comment")]
    public class Comment
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("post_id")]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        [Column("user_id")]
        [ForeignKey("User")]
        public int Userid { get; set; }


        [JsonIgnore]
        public Post Post { get; set; }

        [JsonIgnore]
        public User User { get; set; }

    }
}
