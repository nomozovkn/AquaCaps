using AquaCaps.Application.DTOs;

namespace AquaCaps.Application.Services.AuthService;

public interface IAuthService
{
    Task<long> SignUpAsync(UserCreateDto userCreateDto);
    Task<LogInResponseDto> LogInAsync(UserLogInDto userLoginDto);
    Task<LogInResponseDto> RefreshTokenAsync(RefreshRequestDto request);
    Task LogOutAsync(string refreshToken);
}