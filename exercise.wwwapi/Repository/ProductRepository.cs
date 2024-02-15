using exercise.wwwapi.Data;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Model;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository
{
    public class ProductRepository : IProductRepository
    {
        private DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlogPost>> GetAllProducts()
        {
            return await _context.posts.ToListAsync();
        }

        public async Task<BlogPost> GetProductById(int id)
        {
            return await _context.posts.FindAsync(id);
        }

        public async Task<BlogPost?> AddProduct(BlogPostPayload postPayload)
        {
            var product = new BlogPost
            {
                Title = postPayload.Title,
                Text = postPayload.Text,
                Author = postPayload.Author,
                createdAt = postPayload.DateTime,
            };

            _context.posts.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<BlogPost> UpdateProduct(BlogPutPayload productPutPayload, int id)
        {
            var product = await _context.posts.FindAsync(id);
            if (product == null)
            {
                return null;
            }
            product.Title = productPutPayload.Title;
            product.Text = productPutPayload.Text;
            product.Author = productPutPayload.Author;
            

            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<BlogPost> DeleteProduct(int id)
        {
            var product = await _context.posts.FindAsync(id);
            if (product == null)
            {
                return null;
            }

            _context.posts.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public ApplicationUser? GetUser(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
