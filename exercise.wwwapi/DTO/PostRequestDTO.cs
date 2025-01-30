using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTO
{
    [NotMapped]
    public class PostRequestDTO
    {
        public string Text { get; set; }
       
    }
}
