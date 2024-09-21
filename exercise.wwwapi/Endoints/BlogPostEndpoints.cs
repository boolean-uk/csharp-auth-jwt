using exercise.wwwapi.DTOs.BlogPosts;
using exercise.wwwapi.Helper;
using exercise.wwwapi.Model;
using exercise.wwwapi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace exercise.wwwapi.Endoints
{
    public static class BlogPostEndpoints
    {
        public static void ConfigureBlogPostEndpoints(this WebApplication app)
        {
            var posts = app.MapGroup("/posts");
            posts.MapGet("", GetAllPosts);
            posts.MapPost("", CreatePost);
            posts.MapPut("{id}", UpdatePost);
        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetAllPosts(IDatabaseRepository<BlogPost> service)
        {
            try
            {

                var result = await service.GetAll(bp => bp.Author, bp => bp.BlogComments);

                List<GetBlogPostDTO> posts = new List<GetBlogPostDTO>();

                foreach (var blogPost in result)
                {
                    GetBlogPostDTO blogPostDTO = new GetBlogPostDTO()
                    {
                        AuthorName = blogPost.Author.Name,
                        Text = blogPost.Text,
                        Posted = blogPost.Posted
                    };

                    posts.Add(blogPostDTO);
                }

                return TypedResults.Ok(new Payload<IEnumerable<GetBlogPostDTO>> { data = posts });

            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreatePost(IDatabaseRepository<BlogPost> service, ClaimsPrincipal author, PostBlogPostDTO newPost)
        {
            try
            {

                BlogPost newBlogPost = new BlogPost()
                {
                    AuthorId = author.GetAuthorId(),
                    Text = newPost.Text,
                    Posted = DateOnly.FromDateTime(DateTime.UtcNow),
                    BlogComments = new List<BlogComment>()
                };

                await service.Insert(newBlogPost);

                await service.Save();

                var result = await service.GetAll(bp => bp.Author, bp => bp.BlogComments);

                var target = result.LastOrDefault(p => p.AuthorId == author.GetAuthorId());

                GetBlogPostDTO blogPostDTO = new GetBlogPostDTO()
                {
                    AuthorName = target.Author.Name,
                    Text = target.Text,
                    Posted = target.Posted
                };

                var path = $"/newpost/{target.Author.Name}";

                return TypedResults.Created(path, new Payload<GetBlogPostDTO>() { data = blogPostDTO});
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
        private static async Task<IResult> UpdatePost(IDatabaseRepository<BlogPost> service, ClaimsPrincipal author, int id, PostBlogPostDTO newPost)
        {
            try
            {

                var target = await service.GetById(id);

                if (target == null)
                {
                    return TypedResults.NotFound($"BlogPost with id: {id} not found!");
                }

                if(target.AuthorId !=  author.GetAuthorId())
                {
                    return TypedResults.BadRequest("You are not the author, so you can not edit!");
                }

                target.Text = newPost.Text;

                await service.Update(target);
                await service.Save();

                var result = await service.GetAll(bp => bp.Author, bp => bp.BlogComments);

                var blog = result.FirstOrDefault(bp => bp.Id == id);

                GetBlogPostDTO blogPostDTO = new GetBlogPostDTO()
                {
                    AuthorName = blog.Author.Name,
                    Text = blog.Text,
                    Posted = blog.Posted
                };

                var path = $"/post/updated/{blog.Id}";

                return TypedResults.Created(path, new Payload<GetBlogPostDTO>() { data = blogPostDTO });

            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

    }
}
