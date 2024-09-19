using exercise.wwwapi.Models;
using exercise.wwwapi.Models.DataTransferObjects;
using exercise.wwwapi.Models.DataTransferObjects.RequestDTO;
using exercise.wwwapi.Repository;

namespace exercise.wwwapi.Endpoints
{
    public static class RelationApi
    {
        public static void ConfigureRelationApi(this WebApplication app)
        {
            var relation = app.MapGroup("/relation");
            relation.MapPost("/follow", Follow);
            relation.MapPost("/unfollow", Unfollow);
        }

        public static async Task<IResult> Follow(IRepository<Relation> repo, RelationRequestDTO request)
        {
            IEnumerable<Relation> relations = repo.GetAll();
            if (relations.Any(r => r.FollowerId == request.FollowerId && r.FollowedId == request.FollowedId))
            {
                return Results.Conflict(new Payload<RelationRequestDTO>() { status = "Relation already exists", data = request });
            }

            Relation newRelation = new Relation()
            {
                FollowerId = request.FollowerId,
                FollowedId = request.FollowedId,
            };

            repo.Insert(newRelation);

            Payload<Relation> payload = new Payload<Relation>()
            {
                status = "success",
                data = newRelation
            };
            return Results.Ok(payload);
        }

        public static async Task<IResult> Unfollow(IRepository<Relation> repo, RelationRequestDTO request)
        {
            IEnumerable<Relation> relations = repo.GetAll();
            if (!relations.Any(r => r.FollowerId == request.FollowerId && r.FollowedId == request.FollowedId))
            {
                return Results.Conflict(new Payload<RelationRequestDTO>() { status = "Relation doesn't exists", data = request });
            }

            Relation relation = relations.FirstOrDefault(r => r.FollowerId == request.FollowerId && r.FollowedId == request.FollowedId);

            repo.Delete(relation.Id);
            Payload<Relation> payload = new Payload<Relation>()
            {
                status = "success",
                data = relation
            };
            return Results.Ok(payload);
        }
    }
}
