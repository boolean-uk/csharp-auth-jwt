using exercise.wwwapi.DataTransfer.Request;

namespace exercise.wwwapi.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> Get();
        Task<T> Insert(T entity);
    }
}
