using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("Blogposts")]
    public class BlogPost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Column("userId")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Column("post")]
        public string Post { get; set; }
    }
}
