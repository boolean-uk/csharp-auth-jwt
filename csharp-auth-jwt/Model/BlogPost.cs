using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csharp_auth_jwt.Model
{
    [Table("blogPosts")]
    public class BlogPost
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("text")]
        public string? Text { get; set; }

        [Required]
        [Column("authorId")]
        public string? AuthorId { get; set; }

        [ForeignKey("authorId")]
        public virtual BlogUser Author { get; set; }
    }
}
