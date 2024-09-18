using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("blogposts")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("userid")]
        public string UserId { get; set; }
    }
}
