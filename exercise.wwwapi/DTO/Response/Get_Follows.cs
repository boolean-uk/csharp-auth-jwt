using System.ComponentModel.DataAnnotations.Schema;
using api_cinema_challenge.DTO.Interfaces;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.DTO.Response
{
    public class Get_Follows : DTO_Response<Get_Follows, UserFollows>
    {
        public int FollowerId { get; set; }
        public int FollowingsId { get; set; }

        protected override void _Initialize(UserFollows model)
        {
            FollowerId = model.FollowerId;
            FollowingsId = model.FollowingsId;


        }
    }
}
