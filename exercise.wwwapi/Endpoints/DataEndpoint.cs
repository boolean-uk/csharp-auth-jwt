using exercise.wwwapi.Data;
using exercise.wwwapi.Data.DTO;
using exercise.wwwapi.Data.Enums;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class DataEndpoint
    {
        public static void ConfigureDataEndpoint(this WebApplication app)
        {
            var dataGroup = app.MapGroup("data");
            dataGroup.MapGet("/", GetAllData);
            dataGroup.MapPost("/", CreateData);
            dataGroup.MapPut("/{dataId}", UpdateData);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetAllData(IRepository repository)
        {
            ICollection<GetDataDTO> allData = await repository.GetData();
            return TypedResults.Ok(new ResponseObject<ICollection<GetDataDTO>>() { Status = ResponseStatus.Success, Data = allData });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> CreateData(CreateDataDTO cDTO, ClaimsPrincipal user, IRepository repository)
        {
            ResponseObject<GetDataDTO> createdObject = await repository.CreateData(cDTO, user.Email()!);
            if (createdObject.Status == ResponseStatus.Failure) return TypedResults.BadRequest(createdObject);
            return TypedResults.Created(" ", createdObject);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> UpdateData(int dataId, CreateDataDTO cDTO, ClaimsPrincipal user, IRepository repository)
        {
            ResponseObject<GetDataDTO> createdObject = await repository.UpdateData(dataId, cDTO, user.Email()!);
            if (createdObject.Status == ResponseStatus.Failure) return TypedResults.BadRequest(createdObject);
            return TypedResults.Created(" ", createdObject);
        }
    }
}
