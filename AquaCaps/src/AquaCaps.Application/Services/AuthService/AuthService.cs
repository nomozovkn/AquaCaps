using AquaCaps.Application.DTOs;
using AquaCaps.Application.Interface;
using AquaCaps.Application.MapService;
using AquaCaps.Application.Services.Helpers.Security;
using AquaCaps.Application.Services.TokenService;
using AquaCaps.Core.Errors;
using AquaCaps.Domain.Entity;
using FluentValidation;
using System.Security.Claims;


namespace AquaCaps.Application.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IValidator<UserCreateDto> _userValidator;
    private readonly IValidator<UserLogInDto> _userLoginValidator;
    private readonly IRoleRepository _roleRepository;
    public AuthService(IRefreshTokenRepository refreshTokenRepository, IUserRepository userrepository, ITokenService token, IValidator<UserCreateDto> userValidator, IValidator<UserLogInDto> userLoginValidator, IRoleRepository roleRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userrepository;
        _tokenService = token;
        _userValidator = userValidator;
        _userLoginValidator = userLoginValidator;
        _roleRepository = roleRepository;

    }
    public async Task<long> SignUpAsync(UserCreateDto userCreateDto)
    {
        // 1. Validatsiyani tekshirish
        var validationResult = await _userValidator.ValidateAsync(userCreateDto);
        if (!validationResult.IsValid)
        {
            // ValidationException ni tashlash - yaxshi amaliyot
            throw new ValidationException(validationResult.Errors);
        }

        // 2. Parolni hash qilish
        var (hash, salt) = PasswordHasher.Hasher(userCreateDto.Password);

        // 3. User obyektini yaratish
        var user = new User
        {
            FirstName = userCreateDto.FirstName,
            LastName = userCreateDto.LastName,
            UserName = userCreateDto.UserName,
            Email = userCreateDto.Email,
            PhoneNumber = userCreateDto.PhoneNumber,
            Address = userCreateDto.Address,
            Latitude = userCreateDto.Latitude,
            Longitude = userCreateDto.Longitude,
            IsActive = true,
            Password = hash,
            Salt = salt,
            // RoleId keyingi qadamda o'rnatiladi
        };

        // 4. RoleId ni olish va o'rnatish
        user.RoleId = await _roleRepository.GetRoleIdAsync("User");

        // 5. Userni bazaga qo'shish va id olish
        var userId = await _userRepository.InsertAsync(user);

        return userId;
    }

    public async Task<LogInResponseDto> LogInAsync(UserLogInDto userLoginDto)
    {
        // 1. Kirish DTO ni validatsiyadan o'tkazamiz
        var validationResult = await _userLoginValidator.ValidateAsync(userLoginDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // 2. Foydalanuvchini nomi bo'yicha topamiz
        var user = await _userRepository.SelectByUserNameAsync(userLoginDto.UserName);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        // 3. Parolni tekshirish
        var isPasswordValid = PasswordHasher.Verify(userLoginDto.Password, user.Password, user.Salt);
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        // 4. User obyektini DTO ga aylantirish
        var userDto = MapUserService.ConvertToDto(user);

        // 5. Tokenlar yaratish
        var accessToken = _tokenService.GenerateToken(userDto);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // 6. Refresh tokenni saqlash
        var refreshTokenToDB = new RefreshToken
        {
            Token = refreshToken,
            Expire = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId
        };
        await _refreshTokenRepository.InsertRefreshTokenAsync(refreshTokenToDB);

        // 7. Javobni qaytarish
        return new LogInResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            TokenType = "Bearer",
            Expires = 24 // bu soatlarda amal qilish muddati bo‘lsa aniqrog‘ini `TimeSpan` yoki `DateTime` qilib berish mumkin
        };
    }


    public Task LogOutAsync(string refreshToken) => _refreshTokenRepository.RemoveRefreshTokenAsync(refreshToken);

    public async Task<LogInResponseDto> RefreshTokenAsync(RefreshRequestDto request)
    {
        // 1. Expired token ichidan principal olish
        ClaimsPrincipal? principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null)
            throw new ForbiddenException("Invalid access token.");

        // 2. UserId claimni olish va tekshirish
        var userClaim = principal.FindFirst(c => c.Type == "UserId");
        if (userClaim == null || !long.TryParse(userClaim.Value, out var userId))
            throw new UnauthorizedException("Invalid token claims.");

        // 3. Refresh tokenni bazadan olish va tekshirish
        var refreshToken = await _refreshTokenRepository.SelectRefreshTokenAsync(request.RefreshToken, userId);
        if (refreshToken == null || refreshToken.Expire < DateTime.UtcNow || refreshToken.IsRevoked)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        // 4. Refresh tokenni bekor qilish va bazada yangilash
        refreshToken.IsRevoked = true;
        await _refreshTokenRepository.UpdateRefreshTokenAsync(refreshToken);

        // 5. Foydalanuvchini olish
        var user = await _userRepository.SelectByIdAsync(userId);
        if (user == null)
            throw new UnauthorizedException("User not found.");

        // 6. DTO ga o‘tkazish
        var userGetDto = MapUserService.ConvertToDto(user);

        // 7. Yangi tokenlar yaratish
        var newAccessToken = _tokenService.GenerateToken(userGetDto);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        // 8. Yangi refresh tokenni bazaga kiritish
        var refreshTokenToDB = new RefreshToken
        {
            Token = newRefreshToken,
            Expire = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId
        };
        await _refreshTokenRepository.InsertRefreshTokenAsync(refreshTokenToDB);

        // 9. Javobni qaytarish
        return new LogInResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            TokenType = "Bearer",
            Expires = 24
        };
    }


}