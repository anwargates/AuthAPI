namespace AuthAPI.Controllers
{
    public class TokenRequest
    {
        public string? Username { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
