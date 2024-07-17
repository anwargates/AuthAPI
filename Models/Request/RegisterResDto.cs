namespace AuthAPI.models.request
{
    public class RegisterResDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Identitas { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public RegisterResDto(string username, string email, string password, string identitas, string phoneNumber, string token, string role)
        {
            Username = username;
            Email = email;
            Password = password;
            Identitas = identitas;
            PhoneNumber = phoneNumber;
            Token = token;
            Role = role;
        }

    }

}