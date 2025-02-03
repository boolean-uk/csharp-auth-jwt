using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class CommentDTO
    {
        public int commentId { get; set; }

        public string content { get; set; }

        public int? userId { get; set; } 

        public CommentDTO(Comment comment ) 
        {
            commentId = comment.commentId;
            content = comment.content;  
            userId = comment.userId;    
        }

    }
}
