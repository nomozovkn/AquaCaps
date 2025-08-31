using AquaCaps.Application.DTOs;
using AquaCaps.Application.Services.Helpers.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace AquaCaps.Application.Services.TokenService;

public class TokenService : ITokenService
{
    private IConfiguration Configuration;

    public TokenService(IConfiguration configuration)
    {
        Configuration = configuration.GetSection("Jwt");
    }

    public string GenerateToken(UserDto user)
    {
        var identityClaims = new Claim[]
 {
            new Claim("Id", user?.UserId.ToString() ?? ""),
            new Claim("FirstName", user?.FirstName ?? ""),
            new Claim("LastName", user?.LastName ?? ""),
            new Claim("PhoneNumber", user?.PhoneNumber ?? ""),
            new Claim("UserName", user?.UserName ?? ""),
            new Claim(ClaimTypes.Role, user?.RoleName ?? ""),
            new Claim(ClaimTypes.Email, user?.Email ?? "")
 };


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]!));
        var keyCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiresHours = int.Parse(Configuration["Lifetime"]);
        var token = new JwtSecurityToken(
            issuer: Configuration["Issuer"],
            audience: Configuration["Audience"],
            claims: identityClaims,
            expires: TimeHelper.GetDateTime().AddHours(expiresHours),
            signingCredentials: keyCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Configuration["Issuer"],
            ValidateAudience = true,
            ValidAudience = Configuration["Audience"],
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]!))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
    }

    public string RemoveRefreshTokenAsync(string token)
    {
        throw new NotImplementedException("This method is not implemented ");
    }
}
