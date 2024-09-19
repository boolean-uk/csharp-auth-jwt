using exercise.wwwapi.Data;

namespace exercise.wwwapi.Repository
{
    public interface IRepository<T> where T : class
    {

        public Task<IEnumerable<T>> GetAll();

        public Task<T> Add(T entity);
    }
}
