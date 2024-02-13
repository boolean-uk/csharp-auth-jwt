using exercise.minimalapi.Data;
using exercise.minimalapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.minimalapi.Repositories
{
    public class PostsRepo : IPostsRepo
    {
        private BlogContext _db;
        public PostsRepo(BlogContext db)
        {
            _db = db;
        }

        private async Task<Post?> get(int id)
        {
            return await _db.BlogPosts.FirstOrDefaultAsync(p => p.Id.Equals(id));
        }

        public async Task<Post> Add(string text, string authorId)
        {
            var newPost = await _db.BlogPosts.AddAsync(new Post { Text = text, AuthorId = authorId });
            await _db.SaveChangesAsync();
            return newPost.Entity;
        }

        public async Task<ICollection<Post>> GetAll()
        {

            return await _db.BlogPosts.ToArrayAsync();

        }

        public async Task<ICollection<Post>> GetPostsByUserId(string authorId)
        {
            return await _db.BlogPosts.Where(p => p.AuthorId.Equals(authorId)).ToArrayAsync();
        }
    }
}
