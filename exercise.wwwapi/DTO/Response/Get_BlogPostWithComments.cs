using api_cinema_challenge.DTO.Interfaces;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.DTO.Response
{
    public class Get_BlogPostWithComments : DTO_Response<Get_BlogPostWithComments, BlogPost>, IDTO_Defines_Include<BlogPost>
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Text { get; set; }
        public List<Get_Comment> Comments { get; set; }

        public static Func<IQueryable<BlogPost>, IQueryable<BlogPost>> _includeData()
        {
            return x => x.Include(x => x.Comments).ThenInclude(x => x.User);
        }

        protected override void _Initialize(BlogPost model)
        {
            Id = model.Id;
            AuthorId = model.AuthorId;
            Text = model.Text;
            Comments = Get_Comment.Gets(model.Comments).ToList();
        }

    }
}
