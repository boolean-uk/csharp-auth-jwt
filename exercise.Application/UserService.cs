using AutoMapper;
using exercise.Data.Models;
using exercise.Infrastructure;
using exercise.wwwapi.DTOs;
using Microsoft.AspNetCore.Http;
namespace exercise.Application
{
    public class UserService
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;
        public UserService(IRepository<User> repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetUserDTO>> Get(string id)
        {
            ServiceResponse<GetUserDTO> response = new();
            try
            {
                User user = await _repository.Get(id);
                response.Data = _mapper.Map<GetUserDTO>(user);
            }
            catch (ArgumentException ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetUserDTO>>> GetAll()
        {
            ServiceResponse<List<GetUserDTO>> response = new();
            try
            {
                List<User> users = await _repository.GetAll();
                response.Data = users.Select(_mapper.Map<GetUserDTO>).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        } 
    }
}
