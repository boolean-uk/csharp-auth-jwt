namespace exercise.wwwapi.Repository.Repository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(object id);
        Task<T> Insert(T Entity);
        Task<T> Update(T entity);

    }
}
