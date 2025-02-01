using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text.Json.Serialization;
using api_cinema_challenge.DTO.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTO.Request
{
    public class Delete_Following : DTO_Request_delete<UserFollows>
    {
        [JsonIgnore]
        public int FollowerId{ get; set; }
        [JsonIgnore]
        public int FollowingId{ get; set; }


        protected override Func<IQueryable<UserFollows>, IQueryable<UserFollows>> getId(params object[] id)
        {
            return x => x.Where(x => x.FollowerId == (int)id[0] && x.FollowingsId == (int)id[1]);
        }

        protected override bool VerifRightsToDelete(ClaimsPrincipal user, UserFollows fetchedModel)
        {
            return int.Parse(user.FindFirst(ClaimTypes.Sid).Value) == fetchedModel.FollowerId
                || user.IsInRole("Administrator"); 
        }
    }
}
