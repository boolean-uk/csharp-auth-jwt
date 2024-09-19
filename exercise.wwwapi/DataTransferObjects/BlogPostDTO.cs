using exercise.wwwapi.DataModels;
using exercise.wwwapi.Repository;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataTransferObjects
{
    [NotMapped]
    public class BlogPostDTO
    {
        public string Author { get; set; }
        public string Text { get; set; }

        public BlogPostDTO(BlogPost model)
        {
            Author = model.Author.Username;
            Text = model.Text;
        }
    }
}
