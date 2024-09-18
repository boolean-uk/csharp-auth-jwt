using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public interface IRepository<Model> where Model : class
    {
        public Task<IEnumerable<Model>> GetAll(string[] inclusions);
        public Task<IEnumerable<Model>> GetAll(string[] inclusions, Expression<Func<Model, bool>> predicate);
        public Task<Model> Create(string[] inclusions, Model model);
        public Task<Model> Update(string[] inclusions, Model model);
    }
}
