using api_cinema_challenge.Models.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Models;

namespace api_cinema_challenge.DTO.Interfaces
{
    public interface IDTO_Request_create<DTO_Type,Model_type> 
        where DTO_Type : IDTO_Request_create<DTO_Type, Model_type>
        where Model_type : class, ICustomModel, new()
    {
        public abstract static Task<User?> create(IRepository<Model_type> repo, DTO_Type dto, params object[] pathargs);
    }
}
