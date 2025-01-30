using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

public class User : IBlogEntity
{
    public int Id { get;set; }
    public string Name {get;set;}
    public string Email {get;set;}
    public string Password {get;set;}
    public DateTime CreatedAt {get;set;}
    public DateTime UpdatedAt {get;set;}

    [NotMapped]
    public virtual List<Blog> Blogs {get;set;}

    public void Update(IBlogEntity entity)
    {
        if (entity is User user)
        {
            if (user.Name != null)
                Name = user.Name;
            if (user.Email != null)
                Email = user.Email;
            if (user.Password != null)
                Password = user.Password;
        }
    }
}
