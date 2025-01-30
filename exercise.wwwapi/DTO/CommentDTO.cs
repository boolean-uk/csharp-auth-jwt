using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
