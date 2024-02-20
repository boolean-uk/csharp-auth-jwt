using exercise.wwwapi.DataModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataTransfer.Requests.BlogPost
{
    public class BlogPostPatchRequest
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
