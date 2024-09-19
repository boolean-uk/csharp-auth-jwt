using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.BlogPostDTO
{
    [NotMapped]
    public class PostPutRequest
    {
        public string ?Text { get; set; }

        public string ?AuthorId { get; set; }

    }
}
