using exercise.wwwapi.Model;

namespace exercise.wwwapi.Repository
{
    public interface IRepository
    {
        Task<List<Codes>> GetCodes();
    }
}
