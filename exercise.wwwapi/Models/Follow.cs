using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class Follow
    {
        [Key]
        public int Id  { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("User")]
        public int FollowerId { get; set; }
    }
}
