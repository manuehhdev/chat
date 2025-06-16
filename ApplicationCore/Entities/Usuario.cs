using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities;

public class Usuario : IdentityUser<Guid>
{
    public required string FotoPerfil { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public required string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}