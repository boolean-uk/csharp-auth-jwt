using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("blog_posts")]
    public class BlogPost
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("text")]
        public string Text { get; set; }
        /*[ForeignKey(nameof(ApplicationUser))]
        [Column("fk_author_username")]
        public string Author { get; set; }*/
    }
}
