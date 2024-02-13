using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Numerics;
using System.Threading.Tasks;

namespace WebApplication1.Repository
{
    public interface Irepository
    {
        Task<IEnumerable<BlogPost>> GetBlogPosts();
    }
}
