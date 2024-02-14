using exercise.wwwapi.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DataContext _db;
        private DbSet<T> _entities;

        public Repository(DataContext db)
        {
            _db = db;
            _entities = db.Set<T>();
        }

        public async Task<T> Create(T entity)
        {
            _entities.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> Delete(int id)
        {
            var entity = await _entities.FindAsync(id);
            if (entity == null)
                return null;

            var entityCopy = _db.Entry(entity).CurrentValues.Clone().ToObject() as T;

            _entities.Remove(entity);
            await _db.SaveChangesAsync();

            return entityCopy;
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T?> Get(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<T?> Update(T entity)
        {
            _entities.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
