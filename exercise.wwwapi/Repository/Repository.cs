using exercise.wwwapi.Data;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository
{
    public class Repository : IRepository
    {
        private DatabaseContext _databaseContext;

        public Repository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task<Blogpost> CreateBlogpost(Blogpost post)
        {
            await _databaseContext.Blogposts.AddAsync(post);
            _databaseContext.SaveChanges();
            return post;
        }

        public async Task<IEnumerable<Blogpost>> GetPosts()
        {
            return await _databaseContext.Blogposts.ToListAsync();
        }

        public Task<Blogpost?> UpdateBlogPost(int blogpostId, Blogpost post)
        {
            throw new NotImplementedException();
        }
    }
}
