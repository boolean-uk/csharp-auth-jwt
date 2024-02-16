using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels.Abstract
{
    public abstract class TimestampedEntity
    {
        protected TimestampedEntity()
        {
            this.CreatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            this.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        }

        public void MarkUpdated()
        {
            this.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
