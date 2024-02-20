using exercise.wwwapi.DataTransfers.Requests;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels
{
    [Table("blogs")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [Column("authorId")]
        public string AuthorId { get; set; }
    }
}
