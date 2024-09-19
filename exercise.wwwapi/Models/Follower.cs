using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("Followers")]
    public class Follower
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("followedUserId")]
        public int F_UserId { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [ForeignKey("User")]
        [Column("userId")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
