using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTO
{
    public class BlogDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public UserDTO User { get; set; }
    }
}
