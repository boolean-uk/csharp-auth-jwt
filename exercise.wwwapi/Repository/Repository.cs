using exercise.wwwapi.Data;
using exercise.wwwapi.DataModels;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository
{
    public class Repository<T> : IRepository<T> where T : class, IFood
    {

        private DataContext _db;
        private DbSet<T> _dbSet = null;
        public Repository(DataContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }


        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
           T entity = await _dbSet.FindAsync(id) ?? throw new Exception("Not Found");                   //Or this:
            return entity;
        }

        public async Task<T> Insert(T entity)
        {

            //await _db.Set<T>().AddAsync(entity);
            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task<T> Delete(int id)
        {
            T entity = await GetById(id);
            _dbSet.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;

            /*_db.Entry(entity).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return entity;*/
        }

        public async Task<T> Update(T entity, int id)
        {
            T source = await GetById(id);
            _db.Attach(source);
            _db.Entry(source).CurrentValues.SetValues(entity);
            // _dbSet.Update(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
