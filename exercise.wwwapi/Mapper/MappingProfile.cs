using AutoMapper;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserRequestDTO>();
            CreateMap<User, UserResponseDTO>();
            CreateMap<Post, PostResponseDTO>();
        }
    }
}
