using System.Net;
using System.Security.Claims;
using api_cinema_challenge.Models.Interfaces;
using api_cinema_challenge.Repository;

namespace api_cinema_challenge.DTO.Interfaces
{
    /// <summary>
    /// Special base class for all DTO that updates a entity
    /// </summary>
    public abstract class DTO_Request_update<Model_Type>
        where Model_Type : class,ICustomModel, new()
    {
        protected abstract Func<IQueryable<Model_Type>, IQueryable<Model_Type>> getId(params object[] id);
        protected abstract bool VerifRightsToUpdate(ClaimsPrincipal user, Model_Type fetchedModel);
        protected abstract Model_Type CreateAndReturnUpdatedInstance(Model_Type originalModelData);
        public async Task<Model_Type> Update(IRepository<Model_Type> repo, ClaimsPrincipal user, params object[] id)
        {
            var query = getId(id);
            var fetchedModel = await repo.GetEntry(query);
            if (!VerifRightsToUpdate(user, fetchedModel)) throw new HttpRequestException($"requested object is not owned by user {int.Parse(user.FindFirst(ClaimTypes.Sid)!.Value)}", null, HttpStatusCode.Unauthorized);
            if (fetchedModel == null) throw new HttpRequestException("requested object does not exist", null, HttpStatusCode.NotFound);
            var model = CreateAndReturnUpdatedInstance(fetchedModel);
            
            return await repo.UpdateEntry(query, model);
        }
    }
}
