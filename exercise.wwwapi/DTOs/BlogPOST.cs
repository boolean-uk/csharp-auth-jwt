using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs
{
    public class BlogPOST
    {
        public required string Title { get; set; }
        public required string TextContent { get; set; } 
    }
}
