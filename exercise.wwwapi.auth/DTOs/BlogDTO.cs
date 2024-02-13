using exercise.wwwapi.auth.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.auth.DTOs
{
    public class BlogDTO
    {
        public string AuthorName { get; set; }
        public string Title { get; set; }     
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }

        public BlogDTO(Blog blog) {
            AuthorName = blog.AuthorName;
            Title = blog.Title;
            Description = blog.Description;
            CreatedAt = blog.CreatedAt;
            LastUpdated = blog.LastUpdated;
        }
    }
}
