

using System.ComponentModel.DataAnnotations.Schema;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.CommentDTO
{
    [NotMapped]
    public class CommentResponseDto
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public User Author { get; set; }

        public CommentResponseDto(Comment comment)
        {
            Id = comment.Id;
            Comment = comment.Content;
            Author = comment.User;
        }

    }
}
