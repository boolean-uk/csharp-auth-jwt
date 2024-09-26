using AutoMapper;
using exercise.Application;
using exercise.Data.Models;
using exercise.Infrastructure;
using exercise.wwwapi.DTOs;

namespace exercise.wwwapi.Services
{
    public class PostService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Post> _repository;
        public PostService(IMapper mapper, IRepository<Post> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResponse<List<GetPostDTO>>> GetAllPostsByUser(string userId)
        {
            ServiceResponse<List<GetPostDTO>> response = new();
            try
            {
                List<Post> posts = await _repository.GetAll();
                response.Data = posts.Where(p => p.UserId == userId)
                    .Select(_mapper.Map<GetPostDTO>)
                    .ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        } 

        public async Task<ServiceResponse<GetPostDTO>> AddPost(string userId, AddPostDTO addPostDTO)
        {
            ServiceResponse<GetPostDTO> response = new();
            try
            {
                Post post = _mapper.Map<Post>(addPostDTO);
                post.UserId = userId;
                post = await _repository.Add(post);
                response.Data = _mapper.Map<GetPostDTO>(post);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetPostDTO>> ChangePost(string userId, AddPostDTO addPostDTO, string postId)
        {
            ServiceResponse<GetPostDTO> response = new();
            try
            {
                Post post = await _repository.Get(postId);
                if (!post.UserId.Equals(userId))
                {
                    throw new Exception("You are not the owner of this post!");
                }
                post.Title = addPostDTO.Title;
                post.Description = addPostDTO.Description;
                post = await _repository.Update(post);
                response.Data = _mapper.Map<GetPostDTO>(post);
            } catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
