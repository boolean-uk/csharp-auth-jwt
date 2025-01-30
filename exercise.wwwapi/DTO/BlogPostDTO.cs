using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTO
{
    public class BlogPostDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public AuthorDTO Author { get; set; }

        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
    }
}
