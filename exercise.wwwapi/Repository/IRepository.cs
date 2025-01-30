using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll(
            params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeChains);
        Task<TEntity> Get(
            TKey id, string idField, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeChains);
        Task<TEntity> Get(
            TKey id, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeChains);
        Task<TEntity> Add(TEntity entity);
        Task<IEnumerable<TEntity>> AddRange(params TEntity[] entity);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> Delete(TKey id);
        Task<TEntity> Delete(TEntity entity);
        Task Save();
        Task<TEntity> Find(
            Expression<Func<TEntity, bool>> condition,
            params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeChains);
        Task<IEnumerable<TEntity>> FindAll(
            Expression<Func<TEntity, bool>>? condition = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool ascending = true,
            params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeChains);
    }
}
