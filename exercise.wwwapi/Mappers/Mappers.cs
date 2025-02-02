using System;
using AutoMapper;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.Mappers;

public class Mappers : Profile
{
    public Mappers()
    {
        CreateMap<CreateUserDTO, User>();
        CreateMap<User, GetUserDTO>();
        CreateMap<Blog, GetBlogDTO>();
        CreateMap<User, GetUserFromBlogBTO>();
        CreateMap<Blog, GetBlogFromUserDTO>();
        CreateMap<CreateBlogDTO, Blog>();
        CreateMap<UpdateBlogDTO, Blog>();
        

    }
}
