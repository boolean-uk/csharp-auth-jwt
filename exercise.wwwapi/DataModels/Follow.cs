using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels
{
    [Table("follows")]
    public class Follow
    {
        [ForeignKey("User")]
        [Column("userid")]
        public int UserId { get; set; }
        [ForeignKey("OtherUser")]
        [Column("otheruserid")]
        public int OtherUserId { get; set; }
        public User User { get; set; }
        public User OtherUser { get; set; }
    }
}
