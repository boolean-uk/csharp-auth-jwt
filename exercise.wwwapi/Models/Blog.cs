using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

public class Blog : IBlogEntity
{
    public int Id { get;set; }
    public int UserId {get;set;}
    public string Title {get;set;}
    public string Content {get;set;}
    public DateTime CreatedAt {get;set;}
    public DateTime UpdatedAt {get;set;}
    [NotMapped]
    public virtual User User {get;set;}

    public void Update(IBlogEntity entity)
    {
        if (entity is Blog blog)
        {
            if (blog.Title != "")
                Title = blog.Title;
            if (blog.Content != "")
                Content = blog.Content;
        }
    }
}

