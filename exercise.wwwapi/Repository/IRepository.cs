namespace exercise.wwwapi.Repository;

public interface IRepository<T>
{
    Task<T> Add(T entity);
    Task<IEnumerable<T>> GetAll();
    Task<T?> GetById(object id);
    Task<T> Update(T entity);
    Task<T> Delete(T entity);
}