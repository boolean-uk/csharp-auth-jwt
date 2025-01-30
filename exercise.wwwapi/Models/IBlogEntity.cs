using System;

namespace exercise.wwwapi.Models;

public interface IBlogEntity
{
    public int Id {get;set;}
    public DateTime CreatedAt {get;set;}
    public DateTime UpdatedAt {get;set;}
    
    public void Update(IBlogEntity entity);

}
