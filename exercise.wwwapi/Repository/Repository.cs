using exercise.wwwapi.Data;

namespace exercise.wwwapi.Repository;

public class Repository<T>(DataContext db) : IRepository<T> where T : class
{
    public Task<T> Add(T entity)
    {
        db.Set<T>().Add(entity);
        db.SaveChanges();
        return Task.FromResult(entity);
    }
}