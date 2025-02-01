using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using api_cinema_challenge.DTO.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTO.Request
{
    public class Update_BlogPost : DTO_Request_update<BlogPost>
    {
        public string? Text { get; set; }

        protected override Func<IQueryable<BlogPost>, IQueryable<BlogPost>> getId(params object[] id)
        {
            return x => x.Where(x => x.Id == (int)id[0]);
        }

        protected override BlogPost CreateAndReturnUpdatedInstance(BlogPost originalModelData)
        {
            return new BlogPost
            {
                Id = originalModelData.Id,
                AuthorId = originalModelData.AuthorId,
                Text = this.Text ?? originalModelData.Text,
            };
        }

        protected override bool VerifRightsToUpdate(ClaimsPrincipal user, BlogPost fetchedModel)
        {

            return
                int.Parse(user.FindFirst(ClaimTypes.Sid)!.Value) == fetchedModel.AuthorId
                || user.IsInRole("Administrator"); 
        }
    }
}
