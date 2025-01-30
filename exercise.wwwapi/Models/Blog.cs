using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("blog")]
    public class Blog
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("text")]
        public string TextContent { get; set; }


        [ForeignKey(nameof(User))]
        [Column("user_id")]
        public int UserId { get; set; }

        [NotMapped]
        public User User { get; set; }
        
    }
}
