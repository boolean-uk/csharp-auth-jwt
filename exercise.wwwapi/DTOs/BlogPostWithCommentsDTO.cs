namespace exercise.wwwapi.DTOs
{
    public class BlogPostWithCommentsDTO
    {

        public int Id { get; set; }

        public string text { get; set; }

        public string authorId {  get; set; }

        public ICollection<CommentDTO> comments { get; set; }
    }
}
