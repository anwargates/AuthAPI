using Microsoft.EntityFrameworkCore;
using AuthAPI.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Cryptography;
using System.Text.Json;
using AuthAPI.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using AuthAPI.Models;
using AuthAPI.models.response;
using AuthAPI.models.request;

namespace AuthAPI.Services;
public class AuthService : IAuthService
{
    private readonly MyDbContext _context;
    private readonly ILogger<AuthService> _logger;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    // private readonly IMapper _mapper;
    private static Dictionary<string, string> _refreshTokens = new Dictionary<string, string>();


    public AuthService(MyDbContext context, ILogger<AuthService> logger, IConfiguration configuration, UserManager<User> userManager)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
        _userManager = userManager;
        // _mapper = mapper;
    }

    public async Task<GenericResponse> LoginUserAsync(LoginReqDto loginDto)
    {
        User? existingUser = FindUser(loginDto);

        _logger.LogInformation("Existing User [{}]", JsonSerializer.Serialize(existingUser));

        if (existingUser != null)
        {
            var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, loginDto.Password!);
            // if (IsValidUser(loginDto, existingUser))
            if (isPasswordValid)
            {
                var token = GenerateJwtToken(existingUser);

                _logger.LogInformation("Token response [{}]", JsonSerializer.Serialize(token));

                return new GenericResponse(200, "Login Success", new
                {
                    existingUser.Id,
                    existingUser.UserName,
                    existingUser.Email,
                    existingUser.Identitas,
                    existingUser.PhoneNumber,
                    existingUser.CreatedOn,
                    existingUser.LastLoggedIn,
                    existingUser.Role,
                    token
                });
            }
        }
        else
        {
            return new GenericResponse(404, "User not found");
        }

        return new GenericResponse(401, "Wrong Password");
    }

    public async Task<GenericResponse> RegisterUserAsync(RegisterReqDto regDto)
    {

        try
        {
            _logger.LogInformation("Registering User...");
            // Check if the username or email already exists
            if (await _context.Users.AnyAsync(u => u.UserName == regDto.Username))
            {
                return new GenericResponse(400, "Username already exists", null);
            }

            if (await _context.Users.AnyAsync(u => u.Email == regDto.Email))
            {
                return new GenericResponse(400, "Email already exists", null);
            }

            if (await _context.Users.AnyAsync(u => u.PhoneNumber == regDto.PhoneNumber))
            {
                return new GenericResponse(400, "Phone Number already exists", null);
            }

            // Create a new user
            var user = new User
            {
                UserName = regDto.Username,
                Email = regDto.Email,
                // Password = BCrypt.Net.BCrypt.HashPassword(regDto.Password) // Use a proper hashing method
                // Password = regDto.Password,
                Identitas = regDto.Identitas,
                PhoneNumber = regDto.PhoneNumber,
                Role = regDto.Role
            };
            // _context.Users.Add(user);
            // await _context.SaveChangesAsync();

            var result = await _userManager.CreateAsync(
                        user,
                        regDto.Password!
                    );

            if (!result.Succeeded)
            {
                _logger.LogInformation("Error When Registering User [{}]", result);
                return new GenericResponse(400, "Error When Registering User: " + result);
            }

            string token = GenerateJwtToken(user);

            // var userDto = _mapper.Map<RegisterResDto>(user);
            // userDto.Token = token;
            var resDto = new RegisterResDto(
                user.UserName,
                user.Email,
                "",
                user.Identitas,
                user.PhoneNumber,
                token,
                user.Role.ToString()
            );

            return new GenericResponse(201, "User registered successfully", resDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error When Registering User");
            return new GenericResponse(500, "Error When Registering User");
        }
    }

    public async Task<GenericResponse> RefreshToken(TokenRequest tokenRequest)
    {
        var principal = GetPrincipalFromExpiredToken(tokenRequest.AccessToken);
        var username = principal.Identity.Name;

        _logger.LogInformation(JsonSerializer.Serialize(username));

        if (username != tokenRequest.Username)
        {
            return new GenericResponse(401, "Username tidak sama");
        }

        // var newAccessToken = GenerateJwtToken(username, new List<Claim>(principal.Claims));
        var newAccessToken = "";
        var newRefreshToken = GenerateRefreshToken();

        _refreshTokens[tokenRequest.Username] = newRefreshToken;

        return new GenericResponse(200, "Refresh token success", new
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }

    private string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    private string GenerateJwtToken(User user)
    {
        // var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        // var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: CreateClaims(user),
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: CreateSigningCredentials());

        var tokenHandler = new JwtSecurityTokenHandler();

        _logger.LogInformation("Token created");

        return tokenHandler.WriteToken(token);
    }

    private List<Claim> CreateClaims(User user)
    {
        // var jwtSub = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["JwtRegisteredClaimNamesSub"];
        var jwtSub = _configuration["Jwt:JwtRegisteredClaimNamesSub"];

        try
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwtSub),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            _logger.LogInformation("Creating Claims...");

            return claims;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error Creating Claims");
            throw;
        }
    }

    private User? FindUser(LoginReqDto loginRequest)
    {
        User? user = _context.Users.SingleOrDefault(u => u.Email == loginRequest.Email);
        if (user == null)
        {
            user = _context.Users.SingleOrDefault(u => u.PhoneNumber == loginRequest.PhoneNumber);
        }

        return user;
    }

    private SigningCredentials CreateSigningCredentials()
    {
        // var symmetricSecurityKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"];
        
        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            ),
            SecurityAlgorithms.HmacSha256
        );
    }

    // private bool IsValidUser(LoginReqDto loginRequest, User existingUser)
    // {
    //     // Replace this with your user validation logic
    //     return loginRequest.PhoneNumber == existingUser.PhoneNumber || loginRequest.Email == existingUser.Email && loginRequest.Password == existingUser.Password;
    // }
}