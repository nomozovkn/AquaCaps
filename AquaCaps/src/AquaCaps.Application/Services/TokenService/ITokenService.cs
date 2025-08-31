using AquaCaps.Application.DTOs;
using System.Security.Claims;

namespace AquaCaps.Application.Services.TokenService;

public interface ITokenService
{
    public string GenerateToken(UserDto userDto);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    public string RemoveRefreshTokenAsync(string token);
}