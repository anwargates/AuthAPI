// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthAPI.models;
using AuthAPI.Data;
using Microsoft.EntityFrameworkCore;
using AuthAPI.models.response;
using AuthAPI.models.request;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

// using Microsoft.AspNetCore.Identity.Data;

namespace AuthAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<GenericResponse>> Login([FromBody] LoginReqDto loginRequest)
    {
        try
        {
            _logger.LogInformation("Login request received [{}]", JsonSerializer.Serialize(loginRequest));
            var response = await _authService.LoginUserAsync(loginRequest);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while login the user.");
            return StatusCode(500, new GenericResponse(500, "An error occurred while login the user."));
        }
    }


    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<GenericResponse>> Register(RegisterReqDto registerDto)
    {
        try
        {
            _logger.LogInformation("New User registration request received");
            var response = await _authService.RegisterUserAsync(registerDto);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while registering the user.");
            return StatusCode(500, new GenericResponse(500, "An error occurred while registering the user."));
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<GenericResponse>> Refresh([FromBody] TokenRequest tokenRequest)
    {
        // _logger.LogInformation(JsonSerializer.Serialize(_refreshTokens[tokenRequest.Username]));

        // if (!_refreshTokens.ContainsKey(tokenRequest.Username) || _refreshTokens[tokenRequest.Username] != tokenRequest.RefreshToken)
        // {
        //     return Unauthorized();
        // }

        try
        {
            _logger.LogInformation("Refresh token request received");
            var response = await _authService.RefreshToken(tokenRequest);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while Refreshing token.");
            return StatusCode(500, new GenericResponse(500, "An error occurred while Refresh token."));
        }

    }
}