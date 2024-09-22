using exercise.wwwapi.DTOs.Author;
using exercise.wwwapi.DTOs.BlogPosts;
using exercise.wwwapi.DTOs.UserRelationStatus;
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

            var follows = app.MapGroup("/author");
            follows.MapPost("/follows/{id}", FollowAnAuthor);
            follows.MapPut("/unnfollow/{id}", UnFollowAnAuthor);
            follows.MapGet("/viewallposts", FollowingAuthorsPosts);
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

                return TypedResults.Created(path, new Payload<GetBlogPostDTO>() { data = blogPostDTO });
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

                if (target.AuthorId != author.GetAuthorId())
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

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> FollowAnAuthor(IDatabaseRepository<Author> authorRepository, IDatabaseRepository<UserRelationStatus> userRelationRepository, ClaimsPrincipal author, string id)
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

                await userRelationRepository.Insert(new UserRelationStatus()
                { 
                    FollowerId = author.GetAuthorId(),
                    FollowedId = id,
                    Status = Status.FOLLOWING
                });

                await userRelationRepository.Save();

                var result = await authorRepository.GetAll(a => a.UserRelations);

                var authorTarget = result.FirstOrDefault(a => a.Id == author.GetAuthorId());

                List<GetUserRelationDTO> userRelationDTOs = new List<GetUserRelationDTO>();

                foreach (var f in authorTarget.UserRelations)
                {
                    GetUserRelationDTO urDTO = new GetUserRelationDTO()
                    { 
                        Id = f.Id,
                        FollowerName = authorTarget.Name,
                        FollowedName = f.Followed.Name,
                        Status = f.Status,
                    };

                    userRelationDTOs.Add(urDTO);
                }

                GetAuthorFollowerDTO authorDTO = new GetAuthorFollowerDTO()
                {
                    Name = authorTarget.Name,
                    Following = userRelationDTOs
                };

                var path = $"/author/follows/{id}";

                return TypedResults.Created(path, new Payload<GetAuthorFollowerDTO>() { data = authorDTO });

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
        private static async Task<IResult> UnFollowAnAuthor(IDatabaseRepository<Author> authorRepository, IDatabaseRepository<UserRelationStatus> userRelationRepository, ClaimsPrincipal author, string id)
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
        private static async Task<IResult> FollowingAuthorsPosts(IDatabaseRepository<Author> authorRepository, IDatabaseRepository<BlogPost> blogpostRepository, ClaimsPrincipal author)
        {
            try
            {
                var result = await authorRepository.GetAll(a => a.UserRelations);

                var loggedInAuthor = result.Where(a => a.Id == author.GetAuthorId()).FirstOrDefault();

                List<GetFollowingAuthorsPostsDTO> allPostsDTO = new List<GetFollowingAuthorsPostsDTO>();
                
                    foreach (var ur in loggedInAuthor.UserRelations.Where(ur => ur.Status == Status.FOLLOWING))
                    {
                        List<GetBlogPostDTO> blogPostDTOs = new List<GetBlogPostDTO>();
                        var posts = await blogpostRepository.GetAll();

                        var followedPosts = posts.Where(bp => bp.AuthorId == ur.FollowedId).ToList();

                        foreach (var post in posts)
                        {
                            GetBlogPostDTO blogPostDTO = new GetBlogPostDTO()
                            {
                                AuthorName = ur.Followed.Name,
                                Text = post.Text,
                                Posted = post.Posted
                            };

                            blogPostDTOs.Add(blogPostDTO);
                        }
                        GetFollowingAuthorsPostsDTO fpostsDTO = new GetFollowingAuthorsPostsDTO()
                        {
                            FollowedAuthor = ur.Followed.Name,
                            BlogPosts = blogPostDTOs
                        };
                        allPostsDTO.Add(fpostsDTO);
                    }
                
                return TypedResults.Ok(new Payload<IEnumerable<GetFollowingAuthorsPostsDTO>>() { data = allPostsDTO});
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

    }
}
