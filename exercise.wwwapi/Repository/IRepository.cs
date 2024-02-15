namespace exercise.wwwapi.Repository
{
    public interface IRepository<T>
    {
        public Task<ICollection<T>> GetAll();
        public Task<T?> GetById(int id);
        public Task<T> Add(T entity);
        public Task<T?> Update(T entity);
    }
}
