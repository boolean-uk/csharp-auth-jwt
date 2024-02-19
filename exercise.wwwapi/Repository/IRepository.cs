namespace exercise.wwwapi.Repository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> SelectAll();
        Task<T> SelectById(object id);
        Task<T> Delete(T entity);
        Task<T> Insert(T entity);
        Task<T> Update(T entity);
    }
}
