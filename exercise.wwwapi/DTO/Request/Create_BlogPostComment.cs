using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text.Json.Serialization;
using api_cinema_challenge.DTO.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.DTO.Request
{
    public class Create_BlogPostComment : DTO_Request_create<Comment>, IDTO_Defines_Include<Comment>
    {
        [JsonIgnore]
        public int BlogPostId { get; set; }
        public string Text { get; set; }

        public static Func<IQueryable<Comment>, IQueryable<Comment>> _includeData()
        {
            return x => x.Include(x => x.User);
        }

        public override Comment CreateAndReturnNewInstance(ClaimsPrincipal user, params object[] pathargs)
        {
            return new Comment
            {
                UserId = int.Parse(user.FindFirst(ClaimTypes.Sid).Value),
                BlogPostId = (int)pathargs[0],
                Text = this.Text
            };
        }

        protected override bool CheckConditionForValidCreate(ClaimsPrincipal user, Comment model, params object[] pathargs)
        {
            return model.UserId == int.Parse(user.FindFirst(ClaimTypes.Sid).Value); 
        }

        protected override Func<IQueryable<Comment>, IQueryable<Comment>> GetEntryWithIncludes(Comment createdEntity, params object[] id)
        {
            return x => x.Where(x => x.Id == createdEntity.Id).Include(x => x.User).Include(x => x.BlogPost);
        }
    }
}
