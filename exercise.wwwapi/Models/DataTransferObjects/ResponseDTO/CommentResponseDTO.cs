using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models.DataTransferObjects.ResponseDTO
{
    public class CommentResponseDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public UserResponseDTO Author { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        public CommentResponseDTO(Comment c)
        {
            Id = c.Id;
            Content = c.Content;
            Author = new UserResponseDTO(c.Author);
            CreatedAt = c.CreatedAt;
            UpdatedAt = c.UpdatedAt;
        }
    }
}
