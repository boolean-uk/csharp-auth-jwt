using AutoMapper;
using exercise.Data.Models;
using exercise.wwwapi.DTOs;

namespace exercise.wwwapi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, GetUserDTO>();
            CreateMap<AddUserDTO, User>();

            CreateMap<Post, GetPostDTO>();
            CreateMap<AddPostDTO, Post>();
        }
    }
}
