﻿using AutoMapper;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<User, UserResponseDto>();
            CreateMap<BlogPost, BlogPostRequestDTO>();
        }
    }

}
