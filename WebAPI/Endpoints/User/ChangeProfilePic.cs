using System.Security.Claims;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Endpoints.User;

public class ChangeProfilePic : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPatch("/profile-pic", Handle)
        .WithSummary("Changes a user profile pic")
        .WithRequestValidation<Request>()
        .DisableAntiforgery()
        .RequireAuthorization();
    
    public record Request(IFormFile FotoPerfil);

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.FotoPerfil)
                .NotNull()
                .Must(BeValidImageFile).WithMessage("Invalid file type");
        }
        
        private static bool BeValidImageFile(IFormFile file)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            return allowedTypes.Contains(file.ContentType.ToLower());
        }
    }

    private static async Task<Results<UnauthorizedHttpResult, Ok, BadRequest<string>>> Handle(
        [FromForm] Request request,
        UserManager<Usuario> userManager,
        HttpContext context,
        IImageService imageService)
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
        
        var url = await imageService.Edit(usuario.FotoPerfil,"fotos-usuarioss", request.FotoPerfil);
        usuario.FotoPerfil = url;
        await userManager.UpdateAsync(usuario);
        
        return TypedResults.Ok();
    }
}