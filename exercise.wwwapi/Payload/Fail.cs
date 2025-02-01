using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;

namespace exercise.wwwapi.Payload
{
    /// <summary>
    ///  This is a payload helper to make it easier to return a payload based on bad requests...
    /// </summary>
    static public class Fail
    {
        static private Dictionary<HttpStatusCode, Func<object, IResult>> keys = new Dictionary<HttpStatusCode, Func<object, IResult>>()
        {
            {HttpStatusCode.NotFound  , TypedResults.NotFound},
            {HttpStatusCode.BadRequest  , TypedResults.BadRequest},
            {HttpStatusCode.Conflict , TypedResults.Conflict},
            {HttpStatusCode.OK  , TypedResults.Ok},
            {HttpStatusCode.UnprocessableContent  , TypedResults.UnprocessableEntity},
            {HttpStatusCode.Unauthorized  , obj => TypedResults.Unauthorized()},
            {HttpStatusCode.InternalServerError  ,  TypedResults.InternalServerError }
        };

        static public IResult Payload(string msg, Func<Payload<object, object>, IResult> func)
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
            failLoad.Data = new { ex.Message };

            if (keys.ContainsKey(ex.StatusCode.Value))
                return keys[ex.StatusCode.Value].Invoke(failLoad);

            return TypedResults.BadRequest(failLoad);
        }

        static public IResult Payload(string msg, HttpStatusCode code)
        {
            var failLoad = new Payload<object, object>();
            failLoad.Status = "Failure";
            failLoad.Data = new { Message = msg };

            if (keys.ContainsKey(code))
                return keys[code].Invoke(failLoad);

            return TypedResults.BadRequest(failLoad);
        }
    }
}
