namespace exercise.wwwapi.Repository
{
    public interface IRepository<T> where T : class {

        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Delete(int id);
        Task<T> Insert(T entity);
        Task<T> Update(T entity,int id);

    }
}
