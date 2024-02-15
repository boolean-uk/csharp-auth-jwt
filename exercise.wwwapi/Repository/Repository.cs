
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private IdentityDbContext _db;
        private DbSet<T> _table;

        public Repository(IdentityDbContext dbContext)
        {
            _db = dbContext;
            _table = dbContext.Set<T>();
        }
        public async Task<T> Create(T entity)
        {
            _table.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete(int id)
        {
            var entity = _table.Find(id);
            if (entity != null) 
            {
                throw new InvalidOperationException();
            }

            _table.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Get(int id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _table.ToListAsync();
        }

        public async Task<T> Update(T entity)
        {
            _table.Update(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return entity;

        }
    }
}
