using System.Net;
using api_cinema_challenge.DTO;
using Microsoft.AspNetCore.Http.HttpResults;

namespace exercise.wwwapi.DTO.Response
{
    static public class Fail
    {
        static private Dictionary<HttpStatusCode, Func<object, IResult>> keys = new Dictionary<HttpStatusCode, Func<object, IResult>>()
        {
            {HttpStatusCode.NotFound  , TypedResults.NotFound},
            {HttpStatusCode.BadRequest  , TypedResults.BadRequest},
            {HttpStatusCode.Conflict , TypedResults.Conflict},
            {HttpStatusCode.OK  , TypedResults.Ok},
            {HttpStatusCode.UnprocessableContent  , TypedResults.UnprocessableEntity},
            {HttpStatusCode.InternalServerError  ,  TypedResults.InternalServerError }
        };

        static public IResult Payload(string msg, Func<Payload<object,object>,IResult> func, params object[] args)
        {
            var failLoad = new Payload<object, object>();
            failLoad.Status = "Failure";
            failLoad.Data = new { Message = msg };
            
            return func(failLoad);
        }
        static public IResult Payload(HttpRequestException ex)
        {
            var failLoad = new Payload<object, object>();
            failLoad.Status = "Failure";
            failLoad.Data = new { Message = ex.Message };
            
            if (Fail.keys.ContainsKey(ex.StatusCode.Value))
                return keys[ex.StatusCode.Value].Invoke(failLoad);

            return TypedResults.BadRequest(failLoad);
        }
       
        


    }
}
