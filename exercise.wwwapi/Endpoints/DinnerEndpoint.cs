using AutoMapper;
using exercise.wwwapi.DataModels;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.DinnerRequest;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class DinnerEndpoint
    {
        public static void DinnerEndpointCofiguration(this WebApplication app)
        {

            var dinner = app.MapGroup("dinner");
            dinner.MapGet("/", GetDinners);
            dinner.MapPost("/", AddDinner);
            dinner.MapPut("/{id}", UpdateDinner);
            dinner.MapDelete("/{id}", DeleteDinner);
        }


        //DELETE
        [Authorize(Roles = "Admin")]                        //KANTHEE: THIS is only for ADMIN!!!
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]       //KANTHEE: If unauthorized returns 401.
        public static async Task<IResult> DeleteDinner(IRepository<Dinner> repository, ClaimsPrincipal user, IMapper mapper, int id)
        {

            ServiceReponse<OutDinnerDTO> response = new ServiceReponse<OutDinnerDTO>();

            try
            {
                // source: 
                var source = await repository.Delete(id);
                //Transferring:
                response.data = mapper.Map<OutDinnerDTO>(source);
                response.status = "Valid";

                return response != null ? TypedResults.Ok(new { DateTime = DateTime.Now, User = user.Email(), response }) :
                    TypedResults.BadRequest($"Dinner wasn't deleted");
            }

            catch (Exception ex)
            {
                return TypedResults.NotFound($"Could not find Dinner with Id:{id}");
            }
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetDinners(IRepository<Dinner> repository, IMapper mapper)
        {
            ServiceReponse<OutDinnerDTO> response = new ServiceReponse<OutDinnerDTO>();

            //Source:
            var source = await repository.GetAll();

            //Trannsferring:
            List<OutDinnerDTO> results = source.Select(mapper.Map<OutDinnerDTO>).ToList();
            response.data = mapper.Map<OutDinnerDTO>(source);
            response.status = "Valid";

            return TypedResults.Ok(results);
        }


        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> AddDinner(IRepository<Dinner> repository, IMapper mapper, InDinnerDTO newDinner)
        {

            ServiceReponse<OutDinnerDTO> response = new ServiceReponse<OutDinnerDTO>();

            var results = await repository.GetAll();
            if (results.Any(d => d.Name.Equals(newDinner.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return TypedResults.BadRequest("Dinner with provided name already exists");
            }

            try
            {
                Dinner dinner = mapper.Map<Dinner>(newDinner);

                // source: 
                var source = await repository.Insert(dinner);
                //Transferring:
                response.data = mapper.Map<OutDinnerDTO>(source);
                response.status = "Valid";

                return TypedResults.Created(nameof(AddDinner), new { source.Id, response });
            }

            catch (Exception ex)
            //NOT IN USE
            {
                return TypedResults.BadRequest();
            }



        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> UpdateDinner(IRepository<Dinner> repository, IMapper mapper, int id, InDinnerDTO newDinner)
        {
            ServiceReponse<OutDinnerDTO> response = new ServiceReponse<OutDinnerDTO>();
            var results = await repository.GetAll();
            if (results.Any(d => d.Name.Equals(newDinner.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return TypedResults.BadRequest("Dinner with provided name already exists");
            }

            try
            {
                Dinner dinner = mapper.Map<Dinner>(newDinner);
                // source: 
                var source = await repository.Update(dinner, id);
                //Transferring:
                response.data = mapper.Map<OutDinnerDTO>(source);
                response.status = "Valid";

                return response != null ? TypedResults.Ok(new { response }) :
                    TypedResults.BadRequest($"Couldn't save to the database");  
            }

            catch (Exception ex)
            {
                return TypedResults.NotFound($"Could not find Dinner with Id:{id}");
            }


        }



    }
}
