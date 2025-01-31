using AutoMapper;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.utils;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<BlogPostPost, BlogPost>();
    }
}
