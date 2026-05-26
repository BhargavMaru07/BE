using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WIMS.Application.DTOs;
using WIMS.Application.Interfaces.Common;
using WIMS.Domain.Entity;

namespace WIMS.Application.CommonServices;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public GenerateTokenResponse generateToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        var key = jwtSettings["key"]!;
        var issuer = jwtSettings["Issuer"]!;
        var audience = jwtSettings["Audience"]!;
        var accessTokenExpiration = Convert.ToInt32(jwtSettings["AccessTokenMinutes"]);
        var refreshTokenExpiration = Convert.ToInt32(jwtSettings["RefreshTokenDays"]);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("WarehouseId", user.WarehouseId?.ToString() ?? "")
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var accessTokenExpiry = DateTime.UtcNow
            .AddMinutes(accessTokenExpiration);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: accessTokenExpiry,
            signingCredentials: credentials
        );

        var accessToken = new JwtSecurityTokenHandler()
            .WriteToken(tokenDescriptor);

        var refreshToken = GenerateRefreshToken();

        var refreshTokenExpiry = DateTime.UtcNow
            .AddDays(refreshTokenExpiration);

        return new GenerateTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = refreshTokenExpiry
        };

    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];

        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }
}
