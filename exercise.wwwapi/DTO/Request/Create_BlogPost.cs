using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using api_cinema_challenge.DTO.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.DTO.Request
{
    public class Create_BlogPost : DTO_Request_create<BlogPost>
    {
        public string Text { get; set; }

        public override BlogPost CreateAndReturnNewInstance(ClaimsPrincipal user,params object[] pathargs)
        {
            return new BlogPost
            {
                AuthorId = int.Parse(user.FindFirst(ClaimTypes.Sid).Value),
                Text = this.Text
            };
        }

        protected override Func<IQueryable<BlogPost>, IQueryable<BlogPost>> GetEntryWithIncludes(BlogPost createdEntity, params object[] id)
        {
            return x => x.Where(x => x.Id == createdEntity.Id).Include(x => x.User).Include(x => x.Comments);
        }
    }
}
