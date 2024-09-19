using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.BlogPostDTO
{
    [NotMapped]
    public class PostRequest
    {
        public string Text { get; set; }


    }
}
