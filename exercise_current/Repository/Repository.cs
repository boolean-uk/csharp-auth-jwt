using exercise.wwwapi.Data;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;

namespace exercise.wwwapi.Repository
{
    public class Repository : IRepository
    {
        private DatabaseContext _db;
        public Repository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Posts>> GetAllPosts()
        {
            return await _db.Posts.ToListAsync();
        }
        
        public async Task<Posts?> GetPost(int id)
        {
            return _db.Posts.SingleOrDefault(a => a.Id == id);
        }

        public async Task<Posts> CreatePost(string Text, string AuthorId)
        {
            //Create Post to return
            Posts post = new Posts();
            //Populate the post with payload data
            post.Text = Text;
            post.AuthorId = AuthorId;
            //add post to database and save it + return
            _db.Posts.Add(post);
            _db.SaveChanges();
            return post;
        }
        
        public async Task<Posts?> UpdatePost(int id, string Text, string AuthorId)
        {
            //Load Post to return
            Posts post = await GetPost(id);
            if (post == null)
            {
                return null;
            }
            //Populate the post with payload data
            post.Text = Text;
            post.AuthorId = AuthorId;
            //Save database and return
            _db.SaveChanges();
            return post;
        }
    }
}
