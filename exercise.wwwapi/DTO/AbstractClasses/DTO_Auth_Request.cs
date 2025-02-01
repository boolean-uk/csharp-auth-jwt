using System.Net;
using System.Text.Json.Serialization;
using api_cinema_challenge.Models.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.Payload;

namespace api_cinema_challenge.DTO.Interfaces
{
    /// <summary>
    /// Special base class inherited by anything requireing authentication, returns a payload with token; 
    /// </summary>
    public abstract class DTO_Auth_Request<Model_Type> 
        where Model_Type : class,ICustomModel, new()
    {
        [JsonIgnore]
        private string? _auth_token = null;
        public static async Task<object?> authenticate(DTO_Auth_Request<Model_Type> dto, IRepository<Model_Type> repo, IConfigurationSettings conf)
        {
            Model_Type? model = await dto.ReturnCreatedInstanceModel(repo);
            if (model == null) throw new HttpRequestException("requested object does not exist", null,  HttpStatusCode.NotFound);
            if (!await dto.VerifyRequestedModelAgainstDTO(repo, model)) throw new HttpRequestException("Wrong password", null, HttpStatusCode.NotFound);
            
            dto._auth_token = await dto.CreateAndReturnJWTToken(repo, model, conf);
            return dto._auth_token;
        }
        public static Payload<string, string> toPayloadAuth(DTO_Auth_Request<Model_Type>  dto)
        {
            var p = new Payload<string, string>();
            p.Data = dto._auth_token;
            p.Status = dto._auth_token != null ? "success" : "failure";
            return p;
        }

        protected abstract Task<Model_Type?> ReturnCreatedInstanceModel(IRepository<Model_Type> repo);
        protected abstract Task<bool> VerifyRequestedModelAgainstDTO(IRepository<Model_Type> repo, Model_Type model);
        protected abstract Task<string> CreateAndReturnJWTToken(IRepository<Model_Type> repo, Model_Type model, IConfigurationSettings conf);
    }
}
