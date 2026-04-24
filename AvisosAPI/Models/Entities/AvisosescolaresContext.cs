using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace AvisosAPI.Models.Entities;

public partial class AvisosescolaresContext : DbContext
{
    public AvisosescolaresContext()
    {
    }

    public AvisosescolaresContext(DbContextOptions<AvisosescolaresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumno { get; set; }

    public virtual DbSet<Avisogeneral> Avisogeneral { get; set; }

    public virtual DbSet<Avisopersonal> Avisopersonal { get; set; }

    public virtual DbSet<Clase> Clase { get; set; }

    public virtual DbSet<Maestro> Maestro { get; set; }

  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("alumno");

            entity.HasIndex(e => e.Correo, "Correo").IsUnique();

            entity.HasIndex(e => e.IdClase, "IdClase");

            entity.HasIndex(e => e.NumControl, "NumControl").IsUnique();

            entity.Property(e => e.Contraseña).HasMaxLength(50);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.NumControl).HasMaxLength(6);
            entity.Property(e => e.UltimaVistaBandeja).HasColumnType("datetime");

            entity.HasOne(d => d.IdClaseNavigation).WithMany(p => p.Alumno)
                .HasForeignKey(d => d.IdClase)
                .HasConstraintName("alumno_ibfk_1");
        });

        modelBuilder.Entity<Avisogeneral>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("avisogeneral");

            entity.HasIndex(e => e.IdMaestro, "IdMaestro");

            entity.Property(e => e.Contenido).HasColumnType("text");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaVigencia).HasColumnType("datetime");
            entity.Property(e => e.Titulo).HasMaxLength(200);

            entity.HasOne(d => d.IdMaestroNavigation).WithMany(p => p.Avisogeneral)
                .HasForeignKey(d => d.IdMaestro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("avisogeneral_ibfk_1");
        });

        modelBuilder.Entity<Avisopersonal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("avisopersonal");

            entity.HasIndex(e => e.IdAlumno, "IdAlumno");

            entity.HasIndex(e => e.IdMaestro, "IdMaestro");

            entity.Property(e => e.Contenido).HasColumnType("text");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaLectura).HasColumnType("datetime");
            entity.Property(e => e.Titulo).HasMaxLength(200);

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.Avisopersonal)
                .HasForeignKey(d => d.IdAlumno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("avisopersonal_ibfk_2");

            entity.HasOne(d => d.IdMaestroNavigation).WithMany(p => p.Avisopersonal)
                .HasForeignKey(d => d.IdMaestro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("avisopersonal_ibfk_1");
        });

        modelBuilder.Entity<Clase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clase");

            entity.HasIndex(e => e.IdMaestro, "IdMaestro");

            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.IdMaestroNavigation).WithMany(p => p.Clase)
                .HasForeignKey(d => d.IdMaestro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("clase_ibfk_1");
        });

        modelBuilder.Entity<Maestro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("maestro");

            entity.HasIndex(e => e.Correo, "Correo").IsUnique();

            entity.HasIndex(e => e.NumControl, "NumControl").IsUnique();

            entity.Property(e => e.Contraseña).HasMaxLength(50);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.NumControl).HasMaxLength(4);
            entity.Property(e => e.UltimaVistaBandeja).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
