using System.Net;
using System.Text.Json.Serialization;
using api_cinema_challenge.Models.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Configuration;

namespace api_cinema_challenge.DTO.Interfaces
{
    public abstract class DTO_Auth_Request<Model_Type> 
        where Model_Type : class,ICustomModel, new()
    {
        [JsonIgnore]
        private string? _auth_token = null;
        public static async Task<object?> authenticate(DTO_Auth_Request<Model_Type> dto, IRepository<Model_Type> repo, IConfigurationSettings conf)
        {
            Model_Type? model = await dto.get(repo);
            if (model == null) throw new HttpRequestException("requested object does not exist", null,  HttpStatusCode.NotFound);
            if (!await dto.verify(repo, model)) throw new HttpRequestException("Wrong password", null, HttpStatusCode.NotFound);
            
            dto._auth_token = await dto.createToken(repo, model, conf);
            return dto._auth_token;
        }
        public static Payload<string, string> toPayloadAuth(DTO_Auth_Request<Model_Type>  dto)
        {
            var p = new Payload<string, string>();
            p.Data = dto._auth_token;
            p.Status = dto._auth_token != null ? "success" : "failure";
            return p;
        }

        protected abstract Task<Model_Type?> get(IRepository<Model_Type> repo);
        protected abstract Task<bool> verify(IRepository<Model_Type> repo, Model_Type model);
        protected abstract Task<string> createToken(IRepository<Model_Type> repo, Model_Type model, IConfigurationSettings conf);
    }
}
