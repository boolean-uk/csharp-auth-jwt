using System;

namespace exercise.wwwapi.DTOs;



public class GetBlogDTO
{
    public GetUserFromBlogBTO User { get; set; }
    public int Id {get;set;}
    public string Title { get; set; }
    public string Content { get; set; }  
}


public class GetBlogFromUserDTO
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Username { get; set; }
}

public class CreateBlogDTO
{
    public string Title { get; set; }
    public string Content { get; set; }
}

public class UpdateBlogDTO
{
    public string Title { get; set; }
    public string Content { get; set; }
}