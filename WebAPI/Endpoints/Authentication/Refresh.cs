using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Endpoints.Authentication;

public class Refresh : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/refresh", Handle);

    private static async Task<IResult> Handle(
        HttpContext context, 
        UserManager<Usuario> userManager,
        ITokenService tokenService)
    {
        var refreshToken = context.Request.Cookies["REFRESH_TOKEN"];
        
        var usuario = await userManager.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

        if (usuario is null || usuario.RefreshTokenExpiration < DateTime.UtcNow)
        {
            return TypedResults.Json(
                new { message = "Invalid or expired refresh token" },
                statusCode: StatusCodes.Status401Unauthorized
            );
        }
        
        await tokenService.GenerateAndUpdateTokensAsync(usuario, userManager);
        
        return TypedResults.Ok();
       
    }
}