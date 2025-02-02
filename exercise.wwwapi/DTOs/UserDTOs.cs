using System;

namespace exercise.wwwapi.DTOs;



public class CreateUserDTO
{
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}

public class GetUserDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public List<GetBlogFromUserDTO> Blogs { get; set; }
}

public class GetUserFromBlogBTO
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class LogInDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}


