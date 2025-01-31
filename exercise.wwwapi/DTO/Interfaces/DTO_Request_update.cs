using System.Net;
using api_cinema_challenge.Models.Interfaces;
using api_cinema_challenge.Repository;

namespace api_cinema_challenge.DTO.Interfaces
{
    public abstract class DTO_Request_update<Model_Type>
        where Model_Type : class,ICustomModel, new()
    {
        protected abstract Func<IQueryable<Model_Type>, IQueryable<Model_Type>> getId(params object[] id);
        protected abstract Model_Type returnUpdatedInstanceModel(Model_Type originalModelData);
        public async Task<Model_Type> Update(IRepository<Model_Type> repo, params object[] id)
        {
            var query = getId(id);
            var fetchedModel = await repo.GetEntry(query);
            if (fetchedModel == null) throw new HttpRequestException("requested object does not exist", null, HttpStatusCode.NotFound);
            var model = returnUpdatedInstanceModel(fetchedModel);
            
            return await repo.UpdateEntry(query, model);
        }
    }
}
