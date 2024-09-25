using exercise.wwwapi.DTO.GetModels;
using exercise.wwwapi.DTO.PostModels;
using exercise.wwwapi.Helper;
using exercise.wwwapi.Models;
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
    }
}