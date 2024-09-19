using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("blogposts")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [Column("authorId")]
        [ForeignKey("User")]
        public int authorId { get; set; }
        
        public List<Comment> Comments {  get; set; }  

    }
}
