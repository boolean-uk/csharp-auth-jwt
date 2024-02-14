using exercise.wwwapi.Models;
using exercise.wwwapi.Models.OutputModels;
using exercise.wwwapi.Models.PureModels;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogEndpoint
    {
        public static void ConfigureBlogEndpoint(this WebApplication app) 
        {
            var BlogGroup = app.MapGroup("/blog/");

            // Get all
            BlogGroup.MapGet("", GetPosts);

            // Get specific post
            BlogGroup.MapGet("{id}", GetPost);

            // Post 

            // Put

            // Delete
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetPosts(IRepository<Entry> repo) 
        {
            IEnumerable<Entry> entries = await repo.GetAll();
            if (entries.Count() == 0) 
            {
                return TypedResults.NotFound($"No entries could be found in the database.");
            }

            IEnumerable<EntryDTO> entriesOut = entries.Select(s => new EntryDTO(s));

            Payload<IEnumerable<EntryDTO>> payload = new Payload<IEnumerable<EntryDTO>>(entriesOut);
            return TypedResults.Ok(payload);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetPost(IRepository<Entry> repo, int id) 
        {
            Entry? entry = await repo.Get(id);
            if (entry == null) 
            {
                return TypedResults.NotFound($"No entry with provided ID ({id}) found.");
            }

            EntryDTO entryOut = new EntryDTO(entry);
            Payload<EntryDTO> payload = new Payload<EntryDTO>(entryOut);
            return TypedResults.Ok(payload);
        }
    }
}
