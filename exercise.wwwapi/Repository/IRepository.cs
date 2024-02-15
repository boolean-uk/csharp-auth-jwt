namespace exercise.wwwapi.Repository
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll();

        public Task<T> Get(int id);

        public Task<T> Create(T entity);

        public Task<T> Update(T entity);

        public Task<T> Delete(int id);
    }
}
