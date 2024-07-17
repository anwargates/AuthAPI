using System.ComponentModel.DataAnnotations;
using JwtRoleAuthentication.Enums;

namespace AuthAPI.models.request
{
    public class RegisterReqDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Identitas { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.User;
    }

}