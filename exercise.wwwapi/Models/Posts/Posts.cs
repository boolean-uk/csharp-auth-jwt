using System.ComponentModel.DataAnnotations.Schema;
using exercise.wwwapi.Data;

namespace exercise.wwwapi.Models
{
    [Table("posts")]
    public class Posts
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [Column("user_id")]
        public string UserId { get; set; }
        public Users User { get; set; }
    }
}