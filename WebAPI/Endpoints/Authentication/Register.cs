using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Endpoints.Authentication;

public class Register : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/register", Handle)
        .DisableAntiforgery()
        .WithSummary("Registers a user in the database and adds access and refresh tokens to the cookies")
        .WithRequestValidation<Request>();
    
    public record Request(
        string Nombre, 
        string Apellido, 
        string Email, 
        string Password, 
        IFormFile? FotoPerfil);

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty()
                .Length(2, 50)
                .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("The first name must contain only letters");

            RuleFor(x => x.Apellido)
                .NotEmpty()
                .Length(2, 50)
                .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("The last name must contain only letters");

            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("The password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("The password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("The password must contain at least one number");
        }
    }
    
    private static async Task<Results<Ok, Conflict<string>, BadRequest<IEnumerable<string>>>> Handle(
        [FromForm] Request request, 
        UserManager<Usuario> userManager,
        ITokenService tokenService,
        IImageService imageService)
    {
        var userExists = await userManager.FindByEmailAsync(request.Email) != null;
        if (userExists)
        {
            return TypedResults.Conflict("El usuario ya existe");
        }

        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Email = request.Email,
            UserName = request.Email,
        };

        if (request.FotoPerfil is not null && request.FotoPerfil.ContentType.StartsWith("image/"))
        {
            var url = await imageService.Save("fotos-usuarioss", request.FotoPerfil);
            usuario.FotoPerfil = url;
        }
    
        //usuario.PasswordHash = userManager.PasswordHasher.HashPassword(usuario, request.Password);
        var result = await userManager.CreateAsync(usuario, request.Password);
        
        if (!result.Succeeded)
        {
            return  TypedResults.BadRequest(result.Errors.Select(e => e.Description));
        }
        
        await tokenService.GenerateAndUpdateTokensAsync(usuario, userManager);
        
        return TypedResults.Ok();
    }
}
