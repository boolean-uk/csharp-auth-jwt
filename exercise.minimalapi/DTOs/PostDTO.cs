using exercise.minimalapi.Models;

namespace exercise.minimalapi.DTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string AuthorId { get; set; }
        public PostDTO(Post post) 
        {
            Id = post.Id;
            Text = post.Text;
            AuthorId = post.AuthorId;
        }
    }
}
