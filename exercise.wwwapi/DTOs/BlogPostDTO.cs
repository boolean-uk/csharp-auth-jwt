using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs
{
    [NotMapped]
    public class BlogPostResponseDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }

        public BlogPostResponseDTO(BlogPost model)
        {
            Title = model.Title;
            Content = model.Content;
            AuthorName = model.Author.Username;
        }
    }

    [NotMapped]
    public class BlogPostPostDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }

        public BlogPostPostDTO(BlogPost model)
        {
            Title = model.Title;
            Content = model.Content;
            AuthorId = model.AuthorId;
        }
    }

    [NotMapped]
    public class BlogPostPutDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }

        public BlogPostPutDTO(BlogPost model)
        {
            Title = model.Title;
            Content = model.Content;
            AuthorId = model.AuthorId;
        }
    }
}
