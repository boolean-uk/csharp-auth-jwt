using System;

namespace exercise.wwwapi.DTOs;




public class GetBlogDTO
{
    public GetUserFromBlogBTO Author { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }  
}


public class GetBlogFromUserDTO
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Username { get; set; }
}