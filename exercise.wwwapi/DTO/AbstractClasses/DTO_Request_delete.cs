using System.Net;
using System.Security.Claims;
using api_cinema_challenge.Models;
using api_cinema_challenge.Models.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Models;

namespace api_cinema_challenge.DTO.Interfaces
{
    /// <summary>
    /// Special base class for all DTO that preforms a deletion
    /// </summary>
    public abstract class DTO_Request_delete<Model_Type>
        where Model_Type : class, ICustomModel, new()
    {
        protected abstract Func<IQueryable<Model_Type>, IQueryable<Model_Type>> getId(params object[] id);
        protected abstract bool VerifRightsToDelete(ClaimsPrincipal user, Model_Type fetchedModel);
        public async Task<Model_Type> Delete(IRepository<Model_Type> repo, ClaimsPrincipal user,params object[] id)
        {
            var query = getId(id);
            var fetchedModel = await repo.GetEntry(query);
            if (fetchedModel == null) throw new HttpRequestException("requested object does not exist", null, HttpStatusCode.NotFound);
            if (!VerifRightsToDelete(user, fetchedModel)) throw new HttpRequestException($"requested object is not owned by user {int.Parse(user.FindFirst(ClaimTypes.Sid)!.Value)}", null, HttpStatusCode.Unauthorized);

            return await repo.DeleteEntry(query);
        }
    }
}
