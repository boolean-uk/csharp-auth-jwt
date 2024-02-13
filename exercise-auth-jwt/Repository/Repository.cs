using exercise_auth_jwt.DatabaseContext;
using exercise_auth_jwt.DataModels;
using Microsoft.EntityFrameworkCore;

namespace exercise_auth_jwt.Repository
{
    public class Repository : IRepository
    {
        private DataContext _dataContext;

        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<BlogPost>> GetAllPosts()
        {
            return await _dataContext.BlogPosts.ToListAsync();
        }

        public async Task<BlogPost> MakeAPost(string text, string authorId)
        {
            BlogPost blogPost = new BlogPost()
            {
                Text = text,
                authorId = authorId
            };

            _dataContext.BlogPosts.Add(blogPost);
            _dataContext.SaveChanges();
            return blogPost;
        }


        public async Task<BlogPost?> EditPost(int id ,string text)
        {
            var blogPost = await _dataContext.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return null;
            }
            
            blogPost.Text = text;
            

            _dataContext.SaveChanges();
            return blogPost;
        }


    }
}
