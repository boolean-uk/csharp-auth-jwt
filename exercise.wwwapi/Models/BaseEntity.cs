using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public abstract class BaseEntity
    {
        [Column("createdAt")]
        public DateTime? CreatedAt { get; set; }
        [Column("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}
