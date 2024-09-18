using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [NotMapped]
    public class BlogResponse
    {
        public int PostId { get; set; }
        public string PostText { get; set; }
        public string user {  get; set; }
    }
}
