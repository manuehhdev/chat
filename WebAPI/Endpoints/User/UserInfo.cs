using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApplicationCore.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Endpoints.User;

public class UserInfo : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/info", Handle)
        .WithSummary("Gets the info of the authenticated user")
        .RequireAuthorization();
    
    private record Response(
        string Nombre, 
        string Apellido, 
        string Email, 
        string FotoPerfil);

    private static async Task<Results<UnauthorizedHttpResult, Ok<Response>>> Handle(HttpContext context, UserManager<Usuario> userManager)
    {
        var usuarioEmail = context.User.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(usuarioEmail))
        {
            return TypedResults.Unauthorized();
        }
        
        var usuario = await userManager.FindByEmailAsync(usuarioEmail);

        if (usuario is null)
        {
            return TypedResults.Unauthorized();
        }
        
        var response = new Response(
            usuario.Nombre, 
            usuario.Apellido, 
            usuario.Email!, 
            usuario.FotoPerfil);
        
        return TypedResults.Ok(response);
    }
}