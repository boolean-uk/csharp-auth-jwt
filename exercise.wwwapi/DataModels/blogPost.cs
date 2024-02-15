using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels
{
    [Table("BlogPost")]
    public class blogPost
    {
        [Column("Id")]
        public int Id { get; set; }
        [Column("text")]
         public string Text { get; set; }

        [Column("AuthorId")]
        public string AuthorId { get; set; }
    }
}
