using api_cinema_challenge.Models;
using api_cinema_challenge.Models.Interfaces;
using api_cinema_challenge.Repository;
using Microsoft.EntityFrameworkCore.Metadata;

namespace api_cinema_challenge.DTO.Interfaces
{
    public interface IDTO_Respons<Y> {
        public void Initialize(Y model);
    }
    public abstract class DTO_Response<DTO_Type, Model_Type> : IDTO_Respons<Model_Type>
        where DTO_Type : DTO_Response<DTO_Type, Model_Type>, new()
        where Model_Type : class, ICustomModel
    {
        public DTO_Response() { }
        public abstract void Initialize(Model_Type model);

        public static async Task<IEnumerable<DTO_Type>> Gets(IRepository<Model_Type> repo, Func<IQueryable<Model_Type>, IQueryable<Model_Type>>? WhereQuery = null)
        {
            IEnumerable<Model_Type> list;
            if (WhereQuery == null) list = await repo.GetEntries();
            else list = await repo.GetEntries(WhereQuery);

            return list.Select(x => { var a = new DTO_Type(); a.Initialize(x); return a; }).ToList();
        }

        public static Payload<DTO_Type,Model_Type> toPayload(Model_Type model, string status = "success")
        {
            var a = new DTO_Type();
            a.Initialize(model);

            var p = new Payload<DTO_Type,Model_Type>();
            p.Data = a;
            p.Status = status;
            return p;
        }
        public static Payload<IEnumerable<DTO_Type>, Model_Type> toPayload(IEnumerable<Model_Type> models, string status = "success")
        {
            var list = models.Select(x => { var a = new DTO_Type(); a.Initialize(x); return a; }).ToList();

            var p = new Payload<IEnumerable<DTO_Type>,Model_Type>();
            p.Data = list;
            p.Status = status;
            return p;
        }
        public static Payload<IEnumerable<DTO_Type>, Model_Type> toPayload(IEnumerable<DTO_Type> modelsDtoList, string status = "success")
        {
            var p = new Payload<IEnumerable<DTO_Type>,Model_Type>();
            p.Data = modelsDtoList;
            p.Status = status;
            return p;
        }
        public static async Task<Payload<IEnumerable<DTO_Type>, Model_Type>> toPayload(IRepository<Model_Type> repo, Func<IQueryable<Model_Type>, IQueryable<Model_Type>>? WhereQuery = null, string status = "success")
        {
            try
            {
                var p = new Payload<IEnumerable<DTO_Type>, Model_Type>();
                p.Data = await Gets(repo, WhereQuery);
                p.Status = status;
                return p;
            }
            catch (HttpRequestException ex)
            {
                var p = new Payload<IEnumerable<DTO_Type>, Model_Type>();
                p.Data = [];
                p.Status = "Failure";
                return p;
            }
            
        }
    }
}
