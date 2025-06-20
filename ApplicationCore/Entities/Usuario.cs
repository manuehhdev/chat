using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities;

public class Usuario : IdentityUser<Guid>
{
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
    public string FotoPerfil { get; set; } = null!;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public string RefreshToken { get; set; } = null!;
    public DateTime RefreshTokenExpiration { get; set; }
        
    public override string ToString()
    {
        return $"{Nombre} {Apellido}";
    }
    
}