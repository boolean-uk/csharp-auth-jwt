using exercise.wwwapi.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace exercise.wwwapi.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(BlogDbContext context)
        {
            // Seed Users if not already exist
            if (!await context.Users.AnyAsync())
            {
                var users = new[]
                {
                    new User
                    {
                        Username = "user1",
                        Email = "user1@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password1!"),
                        Role = "Author"
                    },
                    new User
                    {
                        Username = "user2",
                        Email = "user2@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password2!"),
                        Role = "Author"
                    },
                    new User
                    {
                        Username = "admin",
                        Email = "admin@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                        Role = "Admin"
                    }
                };

                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }

            // Seed BlogPosts if not already exist
            if (!await context.BlogPosts.AnyAsync())
            {
                var user1 = await context.Users.FirstOrDefaultAsync(u => u.Username == "user1");
                var user2 = await context.Users.FirstOrDefaultAsync(u => u.Username == "user2");
                var admin = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");

                if (user1 == null || user2 == null || admin == null)
                {
                    throw new InvalidOperationException("Users not found in the database.");
                }

                var blogPosts = new[]
                {
                    new BlogPost { Text = "First Blog Post", AuthorId = user1.Id.ToString(), AuthorUsername = user1.Username },
                    new BlogPost { Text = "Second Blog Post", AuthorId = user2.Id.ToString(), AuthorUsername = user2.Username },
                    new BlogPost { Text = "Admin Blog Post", AuthorId = admin.Id.ToString(), AuthorUsername = admin.Username },
                };

                await context.BlogPosts.AddRangeAsync(blogPosts);
                await context.SaveChangesAsync();  // Save the blog posts so that their IDs are generated
            }

            // Seed Comments if not already exist
            if (!await context.Comments.AnyAsync())
            {
                var blogPost1 = await context.BlogPosts.FirstOrDefaultAsync(bp => bp.Text == "First Blog Post");
                var blogPost2 = await context.BlogPosts.FirstOrDefaultAsync(bp => bp.Text == "Second Blog Post");
                var blogPost3 = await context.BlogPosts.FirstOrDefaultAsync(bp => bp.Text == "Admin Blog Post");

                if (blogPost1 == null || blogPost2 == null || blogPost3 == null)
                {
                    throw new InvalidOperationException("Blog posts not found in the database.");
                }

                var comments = new[]
                {
                    new Comment { Text = "Great post!", AuthorId = "user2", CreatedAt = DateTime.UtcNow, BlogPostId = blogPost1.Id },
                    new Comment { Text = "Thanks for sharing!", AuthorId = "user1", CreatedAt = DateTime.UtcNow, BlogPostId = blogPost1.Id },
                    new Comment { Text = "I love this post!", AuthorId = "admin", CreatedAt = DateTime.UtcNow, BlogPostId = blogPost1.Id },

                    new Comment { Text = "Nice read!", AuthorId = "admin", CreatedAt = DateTime.UtcNow, BlogPostId = blogPost2.Id },
                    new Comment { Text = "Interesting points!", AuthorId = "user1", CreatedAt = DateTime.UtcNow, BlogPostId = blogPost2.Id },
                    new Comment { Text = "I agree with you!", AuthorId = "user2", CreatedAt = DateTime.UtcNow, BlogPostId = blogPost2.Id },

                    new Comment { Text = "Good insights!", AuthorId = "user1", CreatedAt = DateTime.UtcNow, BlogPostId = blogPost3.Id },
                    new Comment { Text = "I learned a lot!", AuthorId = "user2", CreatedAt = DateTime.UtcNow, BlogPostId = blogPost3.Id },
                    new Comment { Text = "Well written!", AuthorId = "admin", CreatedAt = DateTime.UtcNow, BlogPostId = blogPost3.Id }
                };

                await context.Comments.AddRangeAsync(comments);
                await context.SaveChangesAsync();
            }
        }

    }
}
                