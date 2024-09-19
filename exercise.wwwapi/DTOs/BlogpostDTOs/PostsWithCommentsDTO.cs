using exercise.wwwapi.DTOs.CommentDTOs;

namespace exercise.wwwapi.DTOs.BlogpostDTOs
{
    public class PostsWithCommentsDTO
    {
        public BlogpostResponseDTO BlogPost { get; set; }
        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
    }
}
