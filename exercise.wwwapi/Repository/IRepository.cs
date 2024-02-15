namespace exercise.wwwapi.Repository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(object id);
        Task<T> Delete(T entity);
        Task<T> Insert(T entity);
        Task<T> Update(T entity);
    }
}
