using AutoMapper;
using exercise.wwwapi.Models;
using System.Numerics;
using exercise.wwwapi.Models.DTOs;

public class MappingProfile : Profile
{ 
    public MappingProfile()
    {
        CreateMap<BlogPost, GetBlogPostDTO>();

        CreateMap<PostBlogPostDTO, BlogPost>()
        .ForMember(dest => dest.AuthorId, opt => opt.Ignore());

        CreateMap<PutBlogPostDTO, BlogPost>()
        .ForMember(dest => dest.AuthorId, opt => opt.Ignore());

    }
}

