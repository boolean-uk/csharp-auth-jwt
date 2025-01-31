using System.Linq.Expressions;
using AutoMapper;
using exercise.wwwapi.Data;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private DataContext db;
        private DbSet<T> table;

        public Repository(DataContext db)
        {
            this.db = db;
            table = db.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await table.ToListAsync();
        }

        public async Task<T?> GetById(object id)
        {
            return await table.FindAsync(id);
        }

        public async Task<T?> Insert(T obj)
        {
            var result = table.Add(obj);
            await db.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<T?> Update<U>(IMapper mapper, object id, U source)
        {
            var dest = await GetById(id);
            if (dest == null)
                return null;
            mapper.Map<U, T>(source, dest);
            await db.SaveChangesAsync();
            return dest;
        }

        public async Task<T?> Delete(object id)
        {
            var entity = await GetById(id);
            if (entity == null)
                return null;
            db.Remove(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Exists(object id)
        {
            return (await GetById(id)) != null;
        }

        public async Task<T?> GetBy(Expression<Func<T, bool>> pred)
        {
            return await table.FirstOrDefaultAsync(pred);
        }
    }
}
