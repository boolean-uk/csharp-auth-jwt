using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApplication.Models
{
    [Table("blogposts")]
    public class BlogPost
    {

        [Column("id")]
        public string Id { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("user_id")]
        public string UserId { get; set; }

        //public ApplicationUser ApplicationUser { get; set; }
    }
}
