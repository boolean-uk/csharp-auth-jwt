using exercise.wwwapi.Data;
using exercise.wwwapi.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {
        private DataContext _db;
        private DbSet<TEntity> _table;
        public Repository(DataContext db)
        {
            _db = db;
            _table = _db.Set<TEntity>();
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            await _table.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task<IEnumerable<TEntity>> AddRange(params TEntity[] entity)
        {
            await _table.AddRangeAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Delete(TKey id)
        {
            TEntity entity = await Get(id);
            return await Delete(entity);
        }
        public async Task<TEntity> Delete(TEntity entity)
        {
            _table.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Get(TKey id, string idField, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeChains)
        {
            IQueryable<TEntity> query = GetIncludeTable(includeChains);
            TEntity? entity = await query.FirstOrDefaultAsync(e => Equals(EF.Property<TKey>(e, idField), id));
            return entity ?? throw new EntityNotFoundException($"That ID does not exist for {typeof(TEntity).Name}");
        }
        public async Task<TEntity> Get(TKey id, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeChains)
        {
            return await Get(id, "Id", includeChains);
        }

        public async Task<IEnumerable<TEntity>> GetAll(params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeChains)
        {
            IQueryable<TEntity> query = GetIncludeTable(includeChains);
            return await query.ToListAsync();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<TEntity> Find(Expression<Func<TEntity, bool>> condition, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeChains)
        {
            IQueryable<TEntity> query = GetIncludeTable(includeChains);
            TEntity? entity = await query.FirstOrDefaultAsync(condition);
            return entity ?? throw new EntityNotFoundException($"Could not find entity that fufills that condition for {typeof(TEntity).Name}");
        }
        public async Task<IEnumerable<TEntity>> FindAll(
            Expression<Func<TEntity, bool>>? condition = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool ascending = true,
            params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeChains)
        {
            IQueryable<TEntity> query = GetIncludeTable(includeChains);

            if (condition != null)
            {
                query = query.Where(condition);
            }
            if (orderBy != null)
            {
                query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            }
            return await query.ToListAsync();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _table.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return entity;
        }

        private IQueryable<TEntity> GetIncludeTable(params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeChains)
        {
            IQueryable<TEntity> query = _table;
            foreach (var includeChain in includeChains)
            {
                query = includeChain(query);
            }
            return query;
        }
    }
}
