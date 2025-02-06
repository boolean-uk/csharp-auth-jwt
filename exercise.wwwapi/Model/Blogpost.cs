using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Model;


/*
Recommended BlogPost model: id, text, authorId (note: this should be a string, to accommodate the UUID)
*/
[Table("blogposts")]
public class Blogpost
{
    [Column("id")]
    public int Id { get; set; }
    [Column("text")]
    public string Text { get; set; }
    [Column("authorId")]
    public string AuthorId { get; set; }
}
