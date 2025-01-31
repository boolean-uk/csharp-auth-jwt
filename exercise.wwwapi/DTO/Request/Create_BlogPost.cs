using System.ComponentModel.DataAnnotations.Schema;
using api_cinema_challenge.DTO.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTO.Request
{
    public class Create_BlogPost : DTO_Request_create<BlogPost>
    {
        public int AuthorId { get; set; }
        public string Text { get; set; }

        public override BlogPost returnNewInstanceModel(params object[] pathargs)
        {
            return new BlogPost
            {
                AuthorId = this.AuthorId,
                Text = this.Text
            };
        }
    }
}
