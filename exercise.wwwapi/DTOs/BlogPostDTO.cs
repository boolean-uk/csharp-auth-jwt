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
