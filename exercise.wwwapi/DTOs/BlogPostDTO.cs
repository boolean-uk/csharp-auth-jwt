using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs
{
    [NotMapped]
    public class BlogPostResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }

        public BlogPostResponseDTO(BlogPost model)
        {
            Id = model.Id;
            Title = model.Title;
            Content = model.Content;
            AuthorName = model.Author.Username;
        }
    }

    [NotMapped]
    public class AuthorBlogPostsResponseDTO
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public List<BlogPostResponseDTOAuthorLess> blogPosts { get; set; } = new List<BlogPostResponseDTOAuthorLess>();
        public AuthorBlogPostsResponseDTO(User model)
        {
            AuthorId = model.Id;
            AuthorName = model.Username;
        }
    }

    [NotMapped]
    public class BlogPostResponseDTOAuthorLess
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public BlogPostResponseDTOAuthorLess(BlogPost model)
        {
            Id = model.Id;
            Title = model.Title;
            Content = model.Content;
        }
    }

    [NotMapped]
    public class BlogPostPostDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

    [NotMapped]
    public class BlogPostPutDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
    }
}
