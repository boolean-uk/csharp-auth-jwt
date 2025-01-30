using api_cinema_challenge.Models.Interfaces;
using api_cinema_challenge.Repository;

namespace api_cinema_challenge.DTO.Interfaces
{
    public interface IDTO_Request_update<DTO_Type,Model_Type> 
        where DTO_Type : IDTO_Request_update<DTO_Type, Model_Type>
        where Model_Type : class,ICustomModel, new()
    {
        public abstract static Task<Model_Type?> update(DTO_Type dto, IRepository<Model_Type> repo, params object[] id);
    }
}
