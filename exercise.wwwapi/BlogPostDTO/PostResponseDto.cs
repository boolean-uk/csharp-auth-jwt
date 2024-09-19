using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.BlogPostDTO
{
    [NotMapped]
    public class PostResponseDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public User Author { get; set; }

        public PostResponseDto(Post post)
        {
            Id = post.Id;
            Text = post.Text;
            Author = post.Author;
        }

    }
}
