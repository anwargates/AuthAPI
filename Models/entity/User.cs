using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JwtRoleAuthentication.Enums;
using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Models;

public class User : IdentityUser
{
    // [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    // public override string Id { get; set; } = Guid.NewGuid().ToString();
    // public string Username { get; set; } = string.Empty;
    // public string Email { get; set; } = string.Empty;
    // public string Password { get; set; } = string.Empty;
    public string Identitas { get; set; } = string.Empty;
    // public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public DateTime LastLoggedIn { get; set; }
    public Role Role { get; set; }

    public User()
    {
        CreatedOn = DateTime.UtcNow;
        LastLoggedIn = DateTime.UtcNow;
    }
}