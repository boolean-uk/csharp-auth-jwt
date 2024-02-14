using exercise.wwwapi.Data;
using exercise.wwwapi.Data.DTO;
using exercise.wwwapi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints
{
    public static class DataEndpoint
    {
        public static void ConfigureDataEndpoint(this WebApplication app)
        {
            var dataGroup = app.MapGroup("data");
            dataGroup.MapGet("/", GetAllData);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAllData(IRepository repository)
        {
            ICollection<GetDataDTO> allData = await repository.GetData();
            return TypedResults.Ok(new ResponseObject<ICollection<GetDataDTO>>() { Status = ResponseStatus.Success, Data = allData });
        }
    }
}
