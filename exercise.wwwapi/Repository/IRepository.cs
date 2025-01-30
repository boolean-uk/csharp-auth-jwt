namespace exercise.wwwapi.Repository;

public interface IRepository<T> where T : class
{
    Task<T> Add(T entity);
}