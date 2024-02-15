using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Data.Models
{
    [Table("data")]
    public class DbData
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("description")]
        public string Description { get; set; }

        [Required]
        [Column("creator_id")]
        public string CreatorId { get; set; }
        public ApiUser Creator { get; set; }
    }
}
