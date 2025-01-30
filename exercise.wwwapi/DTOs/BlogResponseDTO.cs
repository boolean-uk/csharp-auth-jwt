using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs
{
    public class BlogResponseDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string TextContent { get; set; }

        public string Authour {  get; set; }
    }
}
