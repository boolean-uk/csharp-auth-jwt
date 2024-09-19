using exercise.wwwapi.CommentDTO;
using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.BlogPostDTO
{
   
    [NotMapped]
    public class PostCommentResponseDto
    {
        public PostResponseDto Post { get; set; }

        public List<CommentResponseDto> Comments { get; set; } = new List<CommentResponseDto>();



    }
}
