using AuthAPI.Controllers;
using AuthAPI.models.request;
using AuthAPI.models.response;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

// namespace AuthAPI.services;
public interface IAuthService
{
    Task<GenericResponse> RegisterUserAsync(RegisterReqDto regDto);
    Task<GenericResponse> LoginUserAsync(LoginReqDto loginDto);
    Task<GenericResponse> RefreshToken(TokenRequest tokenRequest);
}
