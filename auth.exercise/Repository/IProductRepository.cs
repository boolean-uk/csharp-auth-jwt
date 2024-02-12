using auth.exercise.Model;

namespace auth.exercise.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task<Product> AddProduct(ProductPostPayload postPayload);
        Task<Product> UpdateProduct(ProductPutPayload productPutPayload, int id);
        Task<Product> DeleteProduct(int id);

        public ApplicationUser? GetUser(string email);
    }
}