using api_cinema_challenge.Models;
using api_cinema_challenge.Models.Interfaces;
using api_cinema_challenge.Repository;

namespace api_cinema_challenge.DTO.Interfaces
{
    public interface IDTO_Request_delete<DTO_Type, Model_type> 
        where DTO_Type : IDTO_Request_delete<DTO_Type,Model_type>
        where Model_type : class, ICustomModel, new()
    {
        public abstract static Task<Model_type?> delete(IRepository<Model_type> repo, params object[] id);
    }
}
