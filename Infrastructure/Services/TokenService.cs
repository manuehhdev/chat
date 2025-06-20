using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtOptions _jwtOptions;

    public TokenService(IHttpContextAccessor httpContextAccessor, IOptions<JwtOptions> jwtOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtOptions = jwtOptions.Value;
    }
    
    public (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(Usuario usuario)
    {
        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.Secret));

        var credentials = new SigningCredentials(
            signingKey, 
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email)
        };
        
        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationTimeInMinutes);
        
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);
        
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        
        return (jwtToken, expires);
    }
    
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public void WriteAuthTokenAsHttpOnlyCookie(
        string cookieName, string token, DateTime expiration)
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Append(
            cookieName, token, 
            new CookieOptions
            {
                HttpOnly = true,
                Expires = expiration,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                IsEssential = true
            });
    }
    
    public async Task GenerateAndUpdateTokensAsync(Usuario usuario, UserManager<Usuario> userManager)
    {
        var (jwtToken, expiresAtUtc) = GenerateJwtToken(usuario);
        var refreshTokenValue = GenerateRefreshToken();
    
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(7);
    
        usuario.RefreshToken = refreshTokenValue;
        usuario.RefreshTokenExpiration = refreshTokenExpiration;
    
        await userManager.UpdateAsync(usuario);
    
        WriteAuthTokenAsHttpOnlyCookie(
            "ACCESS_TOKEN", jwtToken, expiresAtUtc);
    
        WriteAuthTokenAsHttpOnlyCookie(
            "REFRESH_TOKEN", refreshTokenValue, refreshTokenExpiration);
    }
}