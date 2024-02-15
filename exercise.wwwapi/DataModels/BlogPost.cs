using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels
{
    [Table("BlogPost")]
    public class BlogPost
    {
        [Key] 
        public int Id { get; set; }

        [Column("text")]
        public string Text { get; set; }
        [Column("authorId")]
        [ForeignKey(nameof(ApplicationUser))]
        public string AuthorId { get; set; }

    }
}
