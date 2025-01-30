using api_cinema_challenge.Models;
using api_cinema_challenge.Models.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;

namespace api_cinema_challenge.DTO.Interfaces
{
    public interface IDTO_Respons<Y> {
        public void Initialize(Y model);
    }
    public abstract class DTO_Response<DTO_Type, Model_type> : IDTO_Respons<Model_type>
        where DTO_Type : DTO_Response<DTO_Type, Model_type>, new()
        where Model_type : class, ICustomModel
    {
        public DTO_Response() { }
        public abstract void Initialize(Model_type model);
        
        public static Payload<DTO_Type,Model_type> toPayload(Model_type model, string status = "success")
        {
            var a = new DTO_Type();
            a.Initialize(model);

            var p = new Payload<DTO_Type,Model_type>();
            p.Data = a;
            p.Status = status;
            return p;
        }
        public static Payload<IEnumerable<DTO_Type>, Model_type> toPayload(IEnumerable<Model_type> models, string status = "success")
        {
            var list = models.Select(x => { var a = new DTO_Type(); a.Initialize(x); return a; }).ToList();

            var p = new Payload<IEnumerable<DTO_Type>,Model_type>();
            p.Data = list;
            p.Status = status;
            return p;
        }
    }
}
