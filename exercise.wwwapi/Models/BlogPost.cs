using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.Models
{
    [Table("Blog posts")]
    public class BlogPost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("user")]
        [ForeignKey("user id")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Column("text")]
        public string BlogText { get; set; }
    }
}
