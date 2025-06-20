using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Interfaces;

public interface ITokenService
{
    (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(Usuario usuario);
    string GenerateRefreshToken();
    void WriteAuthTokenAsHttpOnlyCookie(
        string cookieName, string token, DateTime expiration);
    Task GenerateAndUpdateTokensAsync(Usuario usuario, UserManager<Usuario> userManager);
}