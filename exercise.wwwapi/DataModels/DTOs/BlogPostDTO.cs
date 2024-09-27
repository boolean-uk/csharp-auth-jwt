using exercise.wwwapi.DataModels.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels.DTOs
{
    public class BlogPostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public ApplicationUserDTO User { get; set; }
    }
}
