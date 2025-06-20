using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

public class UsersDbContext : IdentityDbContext<Usuario, IdentityRole<Guid>, Guid> // << IdentityUser<Guid>>
{
    public UsersDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(u => u.Nombre).HasMaxLength(150);
            entity.Property(u => u.Apellido).HasMaxLength(150);
            entity.Property(u => u.RefreshToken).IsRequired(false);
            entity.Property(u => u.FotoPerfil).IsRequired(false);
        });
        
        base.OnModelCreating(modelBuilder);
    }
    
}