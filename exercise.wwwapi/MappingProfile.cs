using AutoMapper;
using exercise.wwwapi.Models;
using exercise.wwwapi.DTOs;
using System.Net.Sockets;

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
