namespace exercise.wwwapi.DTOs
{
    public class PostsWithCommentsDTO
    {
        public BlogpostResponseDTO BlogPost {  get; set; }

        public List<CommentDTO> Comments { get; set; }
    }
}
