using Microsoft.EntityFrameworkCore;
using SistemaGS.Model;

namespace SistemaGS.Repository.DBContext;

public partial class DbsistemaGsContext : DbContext
{
    public DbsistemaGsContext()
    {
    }

    public DbsistemaGsContext(DbContextOptions<DbsistemaGsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EstadoAyuda> EstadoAyuda { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ListaItem> ListaItems { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Planilla> Planillas { get; set; }

    public virtual DbSet<Registro> Registros { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstadoAyuda>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__EstadoAy__FBB0EDC1B85062D6");

            entity.Property(e => e.Estado).HasMaxLength(10);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.IdItem).HasName("PK__Item__51E8426224B6A718");

            entity.ToTable("Item");

            entity.HasIndex(e => e.Nombre, "UQ__Item__75E3EFCFCC72E677").IsUnique();

            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.Property(e => e.Nombre).HasMaxLength(80);
            entity.Property(e => e.TipoItem).HasMaxLength(30);
        });

        modelBuilder.Entity<ListaItem>(entity =>
        {
            entity.HasKey(e => e.IdLista).HasName("PK__ListaIte__3A2D5E0EE7092508");

            entity.ToTable("ListaItem");

            entity.Property(e => e.DetalleJson).HasColumnName("DetalleJSON");
            entity.Property(e => e.FechaEntrega)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdItemNavigation).WithMany(p => p.ListaItems)
                .HasForeignKey(d => d.IdItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ListaItem__IdIte__08B54D69");

            entity.HasOne(d => d.IdPlanillaNavigation).WithMany(p => p.ListaItems)
                .HasForeignKey(d => d.IdPlanilla)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ListaItem__IdPla__07C12930");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("PK__Persona__B4ADFE39BD8153B4");

            entity.ToTable("Persona");

            entity.Property(e => e.Cedula).ValueGeneratedNever();
            entity.Property(e => e.Apellido).HasMaxLength(60);
            entity.Property(e => e.DireccionHabitacion).HasMaxLength(150);
            entity.Property(e => e.DireccionTrabajo).HasMaxLength(150);
            entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");
            entity.Property(e => e.Genero)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LugarTrabajo).HasMaxLength(60);
            entity.Property(e => e.Nombre).HasMaxLength(60);
            entity.Property(e => e.Ocupacion).HasMaxLength(30);
            entity.Property(e => e.Profesion).HasMaxLength(30);
            entity.Property(e => e.TelefonoHabitacion).HasMaxLength(12);
            entity.Property(e => e.TelefonoTrabajo).HasMaxLength(12);
        });

        modelBuilder.Entity<Planilla>(entity =>
        {
            entity.HasKey(e => e.IdPlanilla).HasName("PK__Planilla__65FAB8635027593D");

            entity.ToTable("Planilla");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.DescripcionAyuda).HasMaxLength(500);
            entity.Property(e => e.DescripcionJson).HasColumnName("DescripcionJSON");
            entity.Property(e => e.FechaEntrega)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Observaciones).HasMaxLength(300);
            entity.Property(e => e.TipoAyuda).HasMaxLength(30);

            entity.HasOne(d => d.BeneficiarioNavigation).WithMany(p => p.PlanillaBeneficiarioNavigations)
                .HasForeignKey(d => d.Beneficiario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Planilla__Benefi__7D439ABD");

            entity.HasOne(d => d.FuncionarioNavigation).WithMany(p => p.PlanillaFuncionarioNavigations)
                .HasForeignKey(d => d.Funcionario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Planilla__Funcio__7E37BEF6");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Planillas)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Planilla__IdEsta__01142BA1");

            entity.HasOne(d => d.SolicitanteNavigation).WithMany(p => p.PlanillaSolicitanteNavigations)
                .HasForeignKey(d => d.Solicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Planilla__Solici__7B5B524B");
        });

        modelBuilder.Entity<Registro>(entity =>
        {
            entity.HasKey(e => e.IdRegistro).HasName("PK__Registro__FFA45A9987C7B977");

            entity.ToTable("Registro");

            entity.Property(e => e.Accion).HasMaxLength(20);
            entity.Property(e => e.DetalleJson).HasColumnName("DetalleJSON");
            entity.Property(e => e.FechaAccion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TablaAfectada).HasMaxLength(50);

            entity.HasOne(d => d.UsuarioResponsableNavigation).WithMany(p => p.Registros)
                .HasForeignKey(d => d.UsuarioResponsable)
                .HasConstraintName("FK__Registro__Usuari__0E6E26BF");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__2A49584C2CD47B3E");

            entity.ToTable("Rol");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF97A1C4AF24");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.NombreUsuario, "UQ__Usuario__6B0F5AE0444A30FF").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResetearClave).HasDefaultValue(true);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Usuario__IdRol__71D1E811");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
