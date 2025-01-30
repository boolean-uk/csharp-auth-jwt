using AutoMapper;
using exercise.wwwapi.Models;
using exercise.wwwapi.DTOs;

namespace exercise.wwwapi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BlogPost, BlogPostDTO>();
            CreateMap<BlogPostDTO, BlogPost>();
        }
    }
}
