using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("comments")]
    public class Comment
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("comment")]
        public string CommentText {  get; set; }
        [Column("blogpostid")]
        [ForeignKey("BlogPost")]
        public int BlogPostId { get; set; }
        [Column("userid")]
        [ForeignKey("User")]
        public int Userid { get; set; }

        public User User { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
