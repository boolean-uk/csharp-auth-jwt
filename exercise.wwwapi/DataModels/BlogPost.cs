using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels
{
    [Table("blogposts")]
    public class BlogPost
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string text { get; set; }

        [Column("authour_id")]
        public string AuthourId { get; set; }
    }
}
