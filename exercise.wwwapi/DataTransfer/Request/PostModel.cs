using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataTransfer.Request
{
    public class PostModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        
    }
}
