using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text.Json.Serialization;
using api_cinema_challenge.DTO.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTO.Request
{
    public class Create_Following : DTO_Request_create<UserFollows>
    {
        [JsonIgnore]
        public int FollowerId{ get; set; }
        [JsonIgnore]
        public int FollowingId{ get; set; }

        public override UserFollows CreateAndReturnNewInstance(ClaimsPrincipal user, params object[] pathargs)
        {
            return new UserFollows
            {
                //FollowerId = int.Parse(user.FindFirst(ClaimTypes.Sid).Value), // better but does not align with criteria...
                FollowerId = (int)pathargs[0],
                FollowingsId = (int)pathargs[1]
            };
        }
        protected override bool CheckConditionForValidCreate(ClaimsPrincipal user, UserFollows createdModel, params object[] pathargs)
        {
            return int.Parse(user.Claims.First().Value) == createdModel.FollowerId;
        }

        protected override Func<IQueryable<UserFollows>, IQueryable<UserFollows>> GetEntryWithIncludes(UserFollows createdEntity, params object[] id)
        {
            return x => x.Where(x => x.FollowerId == createdEntity.FollowerId && x.FollowingsId == createdEntity.FollowingsId);
        }
    }
}
