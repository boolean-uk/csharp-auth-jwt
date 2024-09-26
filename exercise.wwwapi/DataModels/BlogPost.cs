using exercise.wwwapi.DataModels.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels
{
    [Table("blog_posts")]
    public class BlogPost : TimestampedEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("content")]
        public string Content { get; set; }
    }
}
