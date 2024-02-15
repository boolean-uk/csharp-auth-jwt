using exercise.wwwapi.DataModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.DataTransfer.Response.Entities
{
    public class BlogPostDTO(BlogPost blogPost)
    {
        public int Id { get; set; } = blogPost.Id;
        public string Title { get; set; } = blogPost.Title;
        public string Content { get; set; } = blogPost.Content;
        public string CreatedAt { get; set; } = blogPost.CreatedAt.ToString("yyyy-MM-dd hh:mm:ss");
        public string UpdatedAt { get; set; } = blogPost.UpdatedAt.ToString("yyyy-MM-dd hh:mm:ss");
        public UserDTO User { get; set; } = new UserDTO(blogPost.User);
    }
}
