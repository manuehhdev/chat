using System;
using System.Collections.Generic;
using ApplicationCore.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chats> Chats { get; set; }

    public virtual DbSet<ChatsUsuarios> ChatsUsuarios { get; set; }

    public virtual DbSet<MensajeAudio> MensajeAudio { get; set; }

    public virtual DbSet<MensajeImagen> MensajeImagen { get; set; }

    public virtual DbSet<MensajeTexto> MensajeTexto { get; set; }

    public virtual DbSet<MensajeUbicacion> MensajeUbicacion { get; set; }

    public virtual DbSet<Mensajes> Mensajes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chats>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Chats_Id");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Descripcion).HasMaxLength(100);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ChatsUsuarios>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdChat });

            entity.Property(e => e.IdUsuario).HasMaxLength(36);

            entity.HasOne(d => d.IdChatNavigation).WithMany(p => p.ChatsUsuarios)
                .HasForeignKey(d => d.IdChat)
                .HasConstraintName("FK_ChatsUsuarios_Chat");
        });

        modelBuilder.Entity<MensajeAudio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MensajeAudio_Id");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.UrlAudio).HasMaxLength(500);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.MensajeAudio)
                .HasForeignKey<MensajeAudio>(d => d.Id)
                .HasConstraintName("FK_MensajeAudio_Mensaje");
        });

        modelBuilder.Entity<MensajeImagen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MensajeImagen_Id");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.UrlImagen).HasMaxLength(500);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.MensajeImagen)
                .HasForeignKey<MensajeImagen>(d => d.Id)
                .HasConstraintName("FK_MensajeImagen_Mensaje");
        });

        modelBuilder.Entity<MensajeTexto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MensajeTexto_Id");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.MensajeTexto)
                .HasForeignKey<MensajeTexto>(d => d.Id)
                .HasConstraintName("FK_MensajeTexto_Mensaje");
        });

        modelBuilder.Entity<MensajeUbicacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MensajeUbicacion_Id");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.MensajeUbicacion)
                .HasForeignKey<MensajeUbicacion>(d => d.Id)
                .HasConstraintName("FK_MensajeUbicacion_Mensaje");
        });

        modelBuilder.Entity<Mensajes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Mensajes_Id");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.FechaEnvio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdUsuario).HasMaxLength(36);

            entity.HasOne(d => d.ChatsUsuarios).WithMany(p => p.Mensajes)
                .HasForeignKey(d => new { d.IdUsuario, d.IdChat })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mensajes_UsuarioChat");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
