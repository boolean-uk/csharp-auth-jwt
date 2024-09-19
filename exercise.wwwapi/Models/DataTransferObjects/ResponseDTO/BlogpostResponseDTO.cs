namespace exercise.wwwapi.Models.DataTransferObjects.ResponseDTO
{
    public class BlogpostResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        
        public UserResponseDTO User { get; set; }
        public IEnumerable<CommentResponseDTO> Comments { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public BlogpostResponseDTO(Blogpost p)
        {
            Id = p.Id;
            Title = p.Title;
            Content = p.Content;
            User = new UserResponseDTO(p.Author);
            Comments = p.Comments.Select(c => new CommentResponseDTO(c));
            CreatedAt = p.CreatedAt;
            UpdatedAt = p.UpdatedAt;
        }
    }
}
