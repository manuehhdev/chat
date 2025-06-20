using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Endpoints.Authentication;

public class Login : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
       .MapPost("/login", Handle)
       .WithSummary("Logs in a user and adds access and refresh tokens to the cookies")
       .WithRequestValidation<Request>();
    
    public record Request(string Email, string Password);

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    private static async Task<Results<UnauthorizedHttpResult, Ok>> Handle(
        Request request, 
        UserManager<Usuario> userManager, 
        ITokenService tokenService)
    {
        var usuario = await userManager.FindByEmailAsync(request.Email);
        if (usuario is null || !await userManager.CheckPasswordAsync(usuario, request.Password))
        {
            return TypedResults.Unauthorized();
        }
        
        await tokenService.GenerateAndUpdateTokensAsync(usuario, userManager);
        
        return TypedResults.Ok();
    }
    
}