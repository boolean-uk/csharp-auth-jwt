using WebApplication1.Models;
using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Numerics;
using System.Threading.Tasks;

namespace WebApplication1.Repository
{
    public class Repository : Irepository
    {
        private BlogContext _blogContext;
        public Repository(BlogContext db)
        {
            _blogContext = db;
        }

        public async Task<IEnumerable<BlogPost>> GetBlogPosts()
        {
            return await _blogContext.BlogPosts.ToListAsync();
        }
    }
}
