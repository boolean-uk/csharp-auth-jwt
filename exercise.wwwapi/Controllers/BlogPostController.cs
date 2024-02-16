using exercise.wwwapi.Data;
using exercise.wwwapi.DataTransfer.Request;
using exercise.wwwapi.DataTransfer.Response;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BlogPostController : Controller
    {
        private readonly IRepository<Blogpost> _repository;

        public BlogPostController(IRepository<Blogpost> repository)
        {
            _repository = repository;
        }
        [HttpGet]
        [Route("/blogposts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var result = await _repository.Get();
            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("/blogposts")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostToBlog([FromBody] BlogPostPost post)
        {
            var result = await _repository.Insert(new Blogpost { Text = post.Text });
            return Created("", new BlogPostResponse { Text = result.Text });
        }

    }
}