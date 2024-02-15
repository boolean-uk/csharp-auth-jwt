using Microsoft.AspNetCore.Identity;
using workshop.webapi.Enums;

namespace workshop.webapi.DataModels;

public class ApplicationUser : IdentityUser
{
    public Role Role { get; set; }
}