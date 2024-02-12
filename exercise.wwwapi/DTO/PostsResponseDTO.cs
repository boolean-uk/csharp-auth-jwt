using System.ComponentModel.DataAnnotations.Schema;
using exercise.wwwapi.Models;
namespace exercise.wwwapi.DTO
{
    public class PostsResponseDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Email { get; set; }

        public UserDTO User { get; set; }


        public PostsResponseDTO(Posts Posts)
        {
            Id = Posts.Id;
            Text = Posts.Text;
            User = new UserDTO(Posts.User);

        }

        public static List<PostsResponseDTO> FromRepository(IEnumerable<Posts> Posts)
        {
            var results = new List<PostsResponseDTO>();
            foreach (var Post in Posts)
            {
                results.Add(new PostsResponseDTO(Post));
            }
            return results;
        }
    }
}