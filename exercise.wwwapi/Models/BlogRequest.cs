using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [NotMapped]
    public class BlogRequest
    {
        public string BlogText { get; set; }
    }
}
