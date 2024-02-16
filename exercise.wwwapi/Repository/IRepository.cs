using exercise.wwwapi.DataModels;

namespace exercise.wwwapi.Repository
{
    public interface IRepository<T>
    {
        Task<T> CreateBlogPost(T entity);
        Task<IEnumerable<T>> Get();
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
        Task<T> GetById(int id);


    }

}
