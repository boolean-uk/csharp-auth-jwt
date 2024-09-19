using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("text")]
        public string Text {  get; set; }

        [ForeignKey("BlogPost")]
        [Column("blogPostId")]
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
