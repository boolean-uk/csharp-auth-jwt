using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public interface IRepository<Model> where Model : class
    {
        public Task<IEnumerable<Model>> GetAll();
        public Task<IEnumerable<Model>> GetAll(Expression<Func<Model, bool>> predicate);
        public Task<Model> Get(Expression<Func<Model, bool>> predicate);
        public Task<Model> Create(Model model);
        public Task<Model> Update(Model model);
    }
}
