using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransfer.Requests;
using exercise.wwwapi.Repository;

namespace exercise.wwwapi.Endpoints
{
    public static class StudentEndpoint
    {
        public static void ConfigureStudentEndpoint(this WebApplication app)
        {

            var students = app.MapGroup("students");
            students.MapGet("/", GetAll);
            students.MapPost("/", AddStudent).AddEndpointFilter(async (invocationContext, next) =>
            {
                var student = invocationContext.GetArgument<StudentPostRequest>(1);

                if (string.IsNullOrEmpty(student.FirstName) || string.IsNullOrEmpty(student.LastName))
                {
                    return Results.BadRequest("You must enter a first name AND last name");
                }
                return await next(invocationContext);
            }); ;
            students.MapGet("/{id}", GetById);
            students.MapPut("/{id}", Update);
            students.MapDelete("/{id}", Delete);

        }
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> Delete(IRepository<Student> repository, ClaimsPrincipal user, int id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Student with Id:{id}");
            }
            var result = await repository.Delete(entity);
            return result != null ? TypedResults.Ok(new { DateTime=DateTime.Now, User=user.Email(), Student=new { FirstName = result.FirstName, LastName = result.LastName }}) : TypedResults.BadRequest($"Student wasn't deleted");
        }
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Update(IRepository<Student> repository, int id, StudentPatchRequest student)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Student with Id:{id}");
            }
            entity.LastName = !string.IsNullOrEmpty(student.LastName) ? student.LastName : entity.LastName;
            entity.FirstName = !string.IsNullOrEmpty(student.FirstName) ? student.FirstName : entity.FirstName;

            var result = await repository.Update(entity);

            return result != null ? TypedResults.Ok(new { FirstName = result.FirstName, LastName = result.LastName }) : TypedResults.BadRequest("Couldn't save to the database?!");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAll(IRepository<Student> repository)
        {
            var entities = await repository.Get();
            List<Object> results = new List<Object>();
            foreach (var student in entities)
            {
                results.Add(new { Id = student.Id, FirstName = student.FirstName, LastName = student.LastName });
            }
            return TypedResults.Ok(results);
        }
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetById(IRepository<Student> repository, int id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Student with Id:{id}");
            }
            return TypedResults.Ok(new { FirstName = entity.FirstName, LastName = entity.LastName });
        }
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> AddStudent(IRepository<Student> repository, StudentPostRequest student)
        {
            var results = await repository.Get();

            if (results.Any(x => x.FirstName.Equals(student.FirstName, StringComparison.OrdinalIgnoreCase) && x.LastName.Equals(student.LastName, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.BadRequest("Student with provided name already exists");
            }

            var entity = new Student() { FirstName = student.FirstName, LastName = student.LastName };
            await repository.Insert(entity);
            return TypedResults.Created($"/{entity.Id}", new { FirstName = entity.FirstName, LastName = entity.LastName });

        }
    }
}
