using System.Linq.Expressions;
using exercise.wwwapi.Data;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository;

public class Repository<T>(DataContext db) : IRepository<T> where T : class
{
    public Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includes)
    {
        var query = db.Set<T>().AsQueryable();
        
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        
        return Task.FromResult(query.AsEnumerable());
    }
    
    public Task<T?> Get(Expression<Func<T, bool>> predicate)
    {
        return Task.FromResult(db.Set<T>().FirstOrDefault(predicate));
    }
    
    public Task<T?> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        var query = db.Set<T>().AsQueryable();
        
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        
        return Task.FromResult(query.FirstOrDefault(predicate));
    }
    
    public Task<T> Add(T entity)
    {
        db.Set<T>().Add(entity);
        db.SaveChanges();
        return Task.FromResult(entity);
    }
    
    public Task<T> Update(T entity)
    {
        db.Set<T>().Update(entity);
        db.SaveChanges();
        return Task.FromResult(entity);
    }
}