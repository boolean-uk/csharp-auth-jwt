namespace exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;
using exercise.wwwapi.Enums;

public class ApplicationUser : IdentityUser
{
    public Role Role { get; set; }
}