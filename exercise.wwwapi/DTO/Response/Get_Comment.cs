using api_cinema_challenge.DTO.Interfaces;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.DTO.Response
{
    public class Get_Comment : DTO_Response<Get_Comment, Comment>
    {
        public string poster_name { get; set; }
        public string Text { get; set; }

        protected override void _Initialize(Comment model)
        {
            poster_name = model.User.Username;
            Text = model.Text;
        }
    }
}
