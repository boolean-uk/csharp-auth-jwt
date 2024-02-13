using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using exercise.wwwapi.Data;
using exercise.wwwapi.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository
{
    

    public class Repository : IRepository
    {
        DataContext db;
        public Repository(DataContext db)
        {
            this.db = db;
        }

        public async Task<BlogPost> CreateBlog(BlogPost data)
        {
            db.BlogPosts.Add(data);
            await db.SaveChangesAsync();
            return data;
        }

        public async Task<BlogPost> DeleteBlogPost(int id)
        {
            var blogPost = await db.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                throw new KeyNotFoundException("Blog post not found");
            }

            db.BlogPosts.Remove(blogPost);
            await db.SaveChangesAsync();
            return blogPost;
        }

        public async Task<List<BlogPost>> GetBlogPosts()
        {
            return await db.BlogPosts.Include(b => b.User).ToListAsync();
        }

        public async Task<BlogPost> UpdateBlogPost(int id, BlogPost data)
        {
            BlogPost? blogPost = await db.BlogPosts.Include(b => b.User).FirstOrDefaultAsync(b => b.Id == id);
            if (blogPost == null)
            {throw new KeyNotFoundException("Blog post not found");
                
            }

            blogPost.Title = data.Title;
            blogPost.Body = data.Body;


            await db.SaveChangesAsync();
            return blogPost;
        }

        public async Task<List<ApplicationUser>> GetUsers()
        {
            return await db.Users.ToListAsync();
        }
    }
}