using auth.exercise.Model;
using Microsoft.EntityFrameworkCore;
using auth.exercise.Data;

namespace auth.exercise.Repository
{
    public class ProductRepository : IProductRepository
    {
        private StoreDataContext _context;

        public ProductRepository(StoreDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product?> AddProduct(ProductPostPayload postPayload)
        {
            var product = new Product
            {
                Name = postPayload.Name,
                Price = postPayload.Price,
                Quantity = postPayload.Quantity
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProduct(ProductPutPayload productPutPayload, int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }

            product.Price = productPutPayload.Price;
            product.Quantity = productPutPayload.Quantity;

            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}