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
            if (blog.Title != null)
                Title = blog.Title;
            if (blog.Content != null)
                Content = blog.Content;
            if (blog.CreatedAt != null)
                CreatedAt = blog.CreatedAt;
            if (blog.UpdatedAt != null)
                UpdatedAt = blog.UpdatedAt;
        }
    }
}

