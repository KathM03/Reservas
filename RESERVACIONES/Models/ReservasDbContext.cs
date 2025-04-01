using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RESERVACIONES.Models;

public partial class ReservasDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ReservasDbContext(DbContextOptions<ReservasDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Facturacion> Facturacions { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Unidade> Unidades { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("CADENASQL"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Facturacion>(entity =>
        {
            entity.HasKey(e => e.IdFact).HasName("PK__Facturac__66E1409E697CE6B5");

            entity.ToTable("Facturacion");

            entity.Property(e => e.IdFact).HasColumnName("id_fact");
            entity.Property(e => e.Estado)
                .HasMaxLength(100)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdReserva).HasColumnName("id_reserva");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.Facturacions)
                .HasForeignKey(d => d.IdReserva)
                .HasConstraintName("FK_Reserva");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva).HasName("PK__Reservas__423CBE5DAAB447C8");

            entity.Property(e => e.IdReserva).HasColumnName("id_reserva");
            entity.Property(e => e.Descuento)
                .HasDefaultValueSql("((0))")
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("descuento");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("('A')")
                .IsFixedLength()
                .HasColumnName("estado");
            entity.Property(e => e.FFin)
                .HasColumnType("datetime")
                .HasColumnName("f_fin");
            entity.Property(e => e.FInicio)
                .HasColumnType("datetime")
                .HasColumnName("f_inicio");
            entity.Property(e => e.TipoEstancia)
                .HasMaxLength(100)
                .HasColumnName("tipo_estancia");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");
            entity.Property(e => e.UnidadId).HasColumnName("unidad_id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Unidad).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.UnidadId)
                .HasConstraintName("FK_Unidad");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_Usuario");
        });

        modelBuilder.Entity<Unidade>(entity =>
        {
            entity.HasKey(e => e.IdUnidad).HasName("PK__Unidades__95D7C92B0C5A9CCC");

            entity.Property(e => e.IdUnidad).HasColumnName("id_unidad");
            entity.Property(e => e.Estado)
                .HasMaxLength(100)
                .HasColumnName("estado");
            entity.Property(e => e.NombreUnidad)
                .HasMaxLength(100)
                .HasColumnName("nombre_unidad");
            entity.Property(e => e.PrecioHora)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precioHora");
            entity.Property(e => e.Tipo)
                .HasMaxLength(100)
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3213E83FE552E479");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__AB6E61640094DEFA").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .HasColumnName("apellido");
            entity.Property(e => e.Psswd).HasColumnName("psswd");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("('A')")
                .IsFixedLength()
                .HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Rol)
                .HasMaxLength(100)
                .HasColumnName("rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
