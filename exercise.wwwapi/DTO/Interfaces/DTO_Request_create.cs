using api_cinema_challenge.Models.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Models;

namespace api_cinema_challenge.DTO.Interfaces
{
    public abstract class DTO_Request_create<Model_type>
        where Model_type : class, ICustomModel, new()
    {
        public abstract Model_type returnNewInstanceModel(params object[] pathargs);
        public async Task<Model_type> Create(IRepository<Model_type> repo, params object[] pathargs)
        {
            var model = returnNewInstanceModel(pathargs);
            var createdEntity = await repo.CreateEntry(model);
            if (createdEntity == null) throw new HttpRequestException("Bad Creation request", null, System.Net.HttpStatusCode.BadRequest);

            return createdEntity;
        }
    }
}
