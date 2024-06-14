using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiPasteles.Models;

public partial class PastelesContext : DbContext
{
    public PastelesContext()
    {
    }

    public PastelesContext(DbContextOptions<PastelesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Calificacion> Calificacions { get; set; }

    public virtual DbSet<Pastel> Pastels { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Califica__3213E83FC1F99D51");

            entity.ToTable("Calificacion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Presentacion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sabor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sabor");

            entity.HasOne(d => d.PastelNavigation).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.Pastel)
                .HasConstraintName("FK__Calificac__Paste__5FB337D6");

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.Usuario)
                .HasConstraintName("FK__Calificac__Usuar__5EBF139D");
        });

        modelBuilder.Entity<Pastel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pastel__3213E83F446B6C09");

            entity.ToTable("Pastel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Origen)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Precio)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3213E83F34DEF25A");

            entity.ToTable("Usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("clave");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
