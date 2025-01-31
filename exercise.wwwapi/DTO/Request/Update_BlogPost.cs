using System.ComponentModel.DataAnnotations.Schema;
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

        protected override BlogPost returnUpdatedInstanceModel(BlogPost originalModelData)
        {
            return new BlogPost
            {
                Id = originalModelData.Id,
                AuthorId = originalModelData.AuthorId,
                Text = this.Text ?? originalModelData.Text,
            };
        }
    }
}
