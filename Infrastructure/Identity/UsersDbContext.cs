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
}