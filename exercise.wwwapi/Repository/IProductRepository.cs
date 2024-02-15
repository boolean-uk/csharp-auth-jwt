using exercise.wwwapi.DTO;
using exercise.wwwapi.Model;

namespace exercise.wwwapi.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<BlogPost>> GetAllProducts();
        Task<BlogPost> GetProductById(int id);
        Task<BlogPost> AddProduct(BlogPostPayload postPayload);
        Task<BlogPost> UpdateProduct(BlogPutPayload productPutPayload, int id);
        Task<BlogPost> DeleteProduct(int id);

        public ApplicationUser? GetUser(string email);
    }
}
