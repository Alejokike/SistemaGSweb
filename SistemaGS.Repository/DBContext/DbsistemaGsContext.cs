using SistemaGS.Model;
using Microsoft.EntityFrameworkCore;

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

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<EstadoAyuda> EstadoAyuda { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ListaItem> ListaItems { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Planilla> Planillas { get; set; }

    public virtual DbSet<Registro> Registros { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS; DataBase=DBSISTEMA_GS; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__A3C02A1073A3CDE0");

            entity.HasIndex(e => e.NombreCategoria, "UQ__Categori__A21FBE9FE7D5CB23").IsUnique();

            entity.Property(e => e.NombreCategoria).HasMaxLength(30);
        });

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
            entity.HasKey(e => e.IdLista).HasName("PK__ListaIte__3A2D5E0EBBF0F843");

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
                .HasConstraintName("FK__ListaItem__IdIte__5BAD9CC8");

            entity.HasOne(d => d.IdPlanillaNavigation).WithMany(p => p.ListaItems)
                .HasForeignKey(d => d.IdPlanilla)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ListaItem__IdPla__5AB9788F");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("PK__Persona__B4ADFE3954773287");

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
            entity.HasKey(e => e.IdPlanilla).HasName("PK__Planilla__65FAB86314264A99");

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
                .HasConstraintName("FK__Planilla__Benefi__531856C7");

            entity.HasOne(d => d.FuncionarioNavigation).WithMany(p => p.PlanillaFuncionarioNavigations)
                .HasForeignKey(d => d.Funcionario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Planilla__Funcio__540C7B00");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Planillas)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Planilla__IdEsta__56E8E7AB");

            entity.HasOne(d => d.SolicitanteNavigation).WithMany(p => p.PlanillaSolicitanteNavigations)
                .HasForeignKey(d => d.Solicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Planilla__Solici__51300E55");
        });

        modelBuilder.Entity<Registro>(entity =>
        {
            entity.HasKey(e => e.IdRegistro).HasName("PK__Registro__FFA45A99BC2D187B");

            entity.ToTable("Registro");

            entity.Property(e => e.Accion).HasMaxLength(20);
            entity.Property(e => e.DetalleJson).HasColumnName("DetalleJSON");
            entity.Property(e => e.FechaAccion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TablaAfectada).HasMaxLength(50);

            entity.HasOne(d => d.UsuarioResponsableNavigation).WithMany(p => p.Registros)
                .HasForeignKey(d => d.UsuarioResponsable)
                .HasConstraintName("FK__Registro__Usuari__6BE40491");
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
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF97D98A2DD0");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Perfil, "UQ__Usuario__277B0CDCECDE99E4").IsUnique();

            entity.HasIndex(e => e.NombreUsuario, "UQ__Usuario__6B0F5AE08E679FA1").IsUnique();

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
                .HasConstraintName("FK__Usuario__IdRol__65370702");

            entity.HasOne(d => d.PerfilNavigation).WithOne(p => p.Usuario)
                .HasForeignKey<Usuario>(d => d.Perfil)
                .HasConstraintName("FK__Usuario__Perfil__690797E6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
