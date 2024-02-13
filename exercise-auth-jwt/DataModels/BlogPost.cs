using System.ComponentModel.DataAnnotations.Schema;

namespace exercise_auth_jwt.DataModels
{
    [Table("blogpost")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("author_id")]
        public string authorId { get; set; }

        
    }
}
