using exercise.wwwapi.DTO.GetModels;
using exercise.wwwapi.DTO.GetResponses;
using exercise.wwwapi.DTO.PostModels;
using exercise.wwwapi.Helper;
using exercise.wwwapi.Models;
using exercise.wwwapi.Models.Enums;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        public static void ConfigureBlogPostEndpoints(this WebApplication app)
        {
            var posts = app.MapGroup("/posts");
            posts.MapGet("/", GetAllPosts);
            posts.MapPost("/", CreatePost);
            posts.MapPut("/{id}", UpdatePost);

        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetAllPosts(IRepository<BlogPost> repository)
        {
            try
            {

                var result = await repository.GetAll(bp => bp.Author, bp => bp.BlogComments);
                List<GetBlogPostResponses> posts = new List<GetBlogPostResponses>();

                foreach (var blogPost in result)
                {
                    GetBlogPostResponses blogPostDTO = new GetBlogPostResponses()
                    {
                        AuthorName = blogPost.Author.Name,
                        Text = blogPost.Text,
                        Posted = blogPost.Posted
                    };

                    posts.Add(blogPostDTO);
                }
                return TypedResults.Ok(new Payload<IEnumerable<GetBlogPostResponses>> { data = posts });

            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreatePost(IRepository<BlogPost> repository, ClaimsPrincipal author, BlogPostModel model)
        {
            try
            {

                BlogPost newBlogPost = new BlogPost()
                {
                    AuthorId = author.GetAuthorId(),
                    Text = model.Text,
                    Posted = DateOnly.FromDateTime(DateTime.UtcNow),
                    BlogComments = new List<BlogComments>()
                };

                await repository.Insert(newBlogPost);

                await repository.Save();

                var result = await repository.GetAll(bp => bp.Author, bp => bp.BlogComments);

                var target = result.LastOrDefault(p => p.AuthorId == author.GetAuthorId());

                GetBlogPostResponses blogPostDTO = new GetBlogPostResponses()
                {
                    AuthorName = target.Author.Name,
                    Text = target.Text,
                    Posted = target.Posted
                };

                var path = $"/newpost/{target.Author.Name}";

                return TypedResults.Created(path, new Payload<GetBlogPostResponses>() { data = blogPostDTO });
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> UpdatePost(IRepository<BlogPost> repository, ClaimsPrincipal author, int id, BlogPostModel model)
        {
            try
            {

                var target = await repository.GetById(id);

                if (target == null)
                {
                    return TypedResults.NotFound($"BlogPost with id: {id} not found!");
                }

                if (target.AuthorId != author.GetAuthorId())
                {
                    return TypedResults.BadRequest("You are not the author, so you can not edit!");
                }

                target.Text = model.Text;

                await repository.Update(target);
                await repository.Save();

                var result = await repository.GetAll(bp => bp.Author, bp => bp.BlogComments);

                var blog = result.FirstOrDefault(bp => bp.Id == id);

                GetBlogPostResponses blogPostDTO = new GetBlogPostResponses()
                {
                    AuthorName = blog.Author.Name,
                    Text = blog.Text,
                    Posted = blog.Posted
                };

                var path = $"/post/updated/{blog.Id}";

                return TypedResults.Created(path, new Payload<GetBlogPostResponses>() { data = blogPostDTO });

            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }

        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> FollowAnAuthor(Repository<Author> authorRepository, IRepository<UserRelations> userRelationRepository, ClaimsPrincipal author, string id)
        {
            try
            {

                var targetToFollow = await authorRepository.GetById(id);

                if (targetToFollow == null)
                {
                    return TypedResults.NotFound($"Author with id {id} not found!");
                }

                var all = await authorRepository.GetAll(a => a.UserRelations);

                var follower = all.Where(a => a.Id == author.GetAuthorId()).FirstOrDefault();

                if (follower.UserRelations.Where(ur => ur.FollowedId == id).Any())
                {
                    return TypedResults.BadRequest($"Already following author with id {id}!");
                }

                await userRelationRepository.Insert(new UserRelations()
                {
                    FollowerId = author.GetAuthorId(),
                    FollowedId = id,
                    Status = Status.FOLLOWING
                });

                await userRelationRepository.Save();

                var result = await authorRepository.GetAll(a => a.UserRelations);

                var authorTarget = result.FirstOrDefault(a => a.Id == author.GetAuthorId());

                List<GetUserRelationsResponse> userRelationDTOs = new List<GetUserRelationsResponse>();

                foreach (var f in authorTarget.UserRelations)
                {
                    GetUserRelationsResponse urDTO = new GetUserRelationsResponse()
                    {
                        Id = f.Id,
                        FollowerName = authorTarget.Name,
                        FollowedName = f.Followed.Name,
                        Status = f.Status,
                    };

                    userRelationDTOs.Add(urDTO);
                }

                GetAuthorFollowerResponse authorDTO = new GetAuthorFollowerResponse()
                {
                    Name = authorTarget.Name,
                    Following = userRelationDTOs
                };

                var path = $"/author/follows/{id}";

                return TypedResults.Created(path, new Payload<GetAuthorFollowerResponse>() { data = authorDTO });

            }
            catch (Exception ex)
            {

                return TypedResults.BadRequest(ex.Message);
            }
        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> UnFollowAnAuthor(Repository<Author> authorRepository, IRepository<UserRelations> userRelationRepository, ClaimsPrincipal author, string id)
        {
            try
            {

                var targetToUnfollow = await authorRepository.GetById(id);

                if (targetToUnfollow == null)
                {
                    return TypedResults.NotFound($"Author with id {id} not found!");
                }

                var all = await authorRepository.GetAll(a => a.UserRelations);

                var follower = all.Where(a => a.Id == author.GetAuthorId()).FirstOrDefault();

                if (!follower.UserRelations.Where(ur => ur.FollowedId == id).Any())
                {
                    return TypedResults.BadRequest($"Not following author with id {id}!");
                }

                var relation = follower.UserRelations.Where(ur => ur.FollowedId == id).FirstOrDefault();

                await userRelationRepository.Delete(relation.Id);
                await userRelationRepository.Save();


                return TypedResults.Ok(new Payload<string>() { data = $"UserRelation between {relation.Follower.Name} and {relation.Followed.Name} is deleted!" });

            }
            catch (Exception ex)
            {

                return TypedResults.BadRequest(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> FollowingAuthorsPosts(Repository<Author> authorRepository, Repository<BlogPost> blogpostRepository, ClaimsPrincipal author)
        {
            try
            {
                var result = await authorRepository.GetAll(a => a.UserRelations);

                var loggedInAuthor = result.Where(a => a.Id == author.GetAuthorId()).FirstOrDefault();

                List<GetFollowingAuthorResponse> allPostsDTO = new List<GetFollowingAuthorResponse>();

                foreach (var ur in loggedInAuthor.UserRelations.Where(ur => ur.Status == Status.FOLLOWING))
                {
                    List<GetBlogPostResponses> blogPostDTOs = new List<GetBlogPostResponses>();
                    var posts = await blogpostRepository.GetAll();

                    var followedPosts = posts.Where(bp => bp.AuthorId == ur.FollowedId).ToList();

                    foreach (var post in posts)
                    {
                        GetBlogPostResponses blogPostDTO = new GetBlogPostResponses()
                        {
                            AuthorName = ur.Followed.Name,
                            Text = post.Text,
                            Posted = post.Posted
                        };

                        blogPostDTOs.Add(blogPostDTO);
                    }
                    GetFollowingAuthorResponse fpostsDTO = new GetFollowingAuthorResponse()
                    {
                        FollowedAuthor = ur.Followed.Name,
                        BlogPosts = blogPostDTOs
                    };
                    allPostsDTO.Add(fpostsDTO);
                }

                return TypedResults.Ok(new Payload<IEnumerable<GetFollowingAuthorResponse>>() { data = allPostsDTO });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

    }
}