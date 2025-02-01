using System.Security.Claims;
using api_cinema_challenge.Models.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Models;

namespace api_cinema_challenge.DTO.Interfaces
{
    /// <summary>
    /// Special base class for all DTO that preforms a new Creation
    /// </summary>
    public abstract class DTO_Request_create<Model_type>
        where Model_type : class, ICustomModel, new()
    {
        protected abstract Func<IQueryable<Model_type>, IQueryable<Model_type>> GetEntryWithIncludes(Model_type createdEntity , params object[] id);
        public abstract Model_type CreateAndReturnNewInstance(ClaimsPrincipal user,params object[] pathargs);
        protected virtual bool CheckConditionForValidCreate(ClaimsPrincipal user, Model_type createdModel,params object[] pathargs)
        {
            return true;
        }
        public async Task<Model_type> Create(IRepository<Model_type> repo, ClaimsPrincipal user, params object[] pathargs)
        {
            var model = CreateAndReturnNewInstance(user, pathargs);
            if(!CheckConditionForValidCreate(user, model, pathargs)) throw new HttpRequestException($"Create condition was not met for creating {typeof(Model_type).Name}", null, System.Net.HttpStatusCode.BadRequest);
            var createdEntity = await repo.CreateEntry(model);
            if (createdEntity == null) throw new HttpRequestException("Bad Creation request", null, System.Net.HttpStatusCode.BadRequest);

            return await repo.GetEntry(GetEntryWithIncludes(createdEntity, pathargs));
        }
    }
}
