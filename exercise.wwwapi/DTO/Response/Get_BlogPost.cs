using api_cinema_challenge.DTO.Interfaces;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.DTO.Response
{
    public class Get_BlogPost : DTO_Response<Get_BlogPost, BlogPost>
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Text { get; set; }

        protected override void _Initialize(BlogPost model)
        {
            Id = model.Id;
            AuthorId = model.AuthorId;
            Text = model.Text;
        }
    }
}
