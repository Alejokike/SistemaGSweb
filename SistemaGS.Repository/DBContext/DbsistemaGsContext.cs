using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SistemaGS.Model;

namespace SistemaGS.Repository.DBContext;

public partial class DbsistemaGsContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DbsistemaGsContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbsistemaGsContext(DbContextOptions<DbsistemaGsContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Ayuda> Ayuda { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Registro> Registros { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    //=> optionsBuilder.UseSqlServer("Server=localhost; DataBase=DBSISTEMA_GS; Trusted_Connection=True; TrustServerCertificate=True;");
    => optionsBuilder.UseSqlServer(_configuration.GetConnectionString("CadenaSQL"));
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ayuda>(entity =>
        {
            entity.HasKey(e => e.IdAyuda).HasName("PK__Ayuda__649DCAF9868F6432");

            entity.Property(e => e.Categoria).HasMaxLength(50);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.FechaEntrega)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.FuncionarioNavigation).WithMany(p => p.AyudumFuncionarioNavigations)
                .HasForeignKey(d => d.Funcionario)
                .HasConstraintName("FK__Ayuda__Funcionar__5441852A")
                .IsRequired(false);

            entity.HasOne(d => d.SolicitanteNavigation).WithMany(p => p.AyudumSolicitanteNavigations)
                .HasForeignKey(d => d.Solicitante)
                .HasConstraintName("FK__Ayuda__Solicitan__534D60F1");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.IdTransaccion).HasName("PK__Inventar__334B1F77B3B1E9AD");

            entity.ToTable("Inventario");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(10, 4)");
            entity.Property(e => e.Concepto).HasMaxLength(60);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.TipoOperacion).HasMaxLength(3);
            entity.Property(e => e.Unidad).HasMaxLength(2);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.IdItem).HasName("PK__Item__51E842629B56C374");

            entity.ToTable("Item");

            entity.HasIndex(e => e.Nombre, "UQ__Item__75E3EFCF56253DAC").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValueSql("((1))");
            entity.Property(e => e.Cantidad)
                .HasDefaultValueSql("((0))")
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Categoria).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(80);
            entity.Property(e => e.Unidad)
                .HasMaxLength(2)
                .HasDefaultValueSql("('EU')");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("PK__Persona__B4ADFE390AA28CA7");

            entity.ToTable("Persona");

            entity.Property(e => e.Cedula).ValueGeneratedNever();
            entity.Property(e => e.Apellido).HasMaxLength(60);
            entity.Property(e => e.DireccionHabitacion).HasMaxLength(150);
            entity.Property(e => e.DireccionTrabajo).HasMaxLength(150);
            entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");
            entity.Property(e => e.Genero).HasMaxLength(1);
            entity.Property(e => e.LugarTrabajo).HasMaxLength(60);
            entity.Property(e => e.Nombre).HasMaxLength(60);
            entity.Property(e => e.Ocupacion).HasMaxLength(30);
            entity.Property(e => e.Profesion).HasMaxLength(30);
            entity.Property(e => e.TelefonoHabitacion).HasMaxLength(12);
            entity.Property(e => e.TelefonoTrabajo).HasMaxLength(12);
        });

        modelBuilder.Entity<Registro>(entity =>
        {
            entity.HasKey(e => e.IdRegistro).HasName("PK__Registro__FFA45A9918E41B21");

            entity.ToTable("Registro");

            entity.Property(e => e.Accion).HasMaxLength(12);
            entity.Property(e => e.FechaAccion).HasColumnType("datetime");
            entity.Property(e => e.TablaAfectada).HasMaxLength(12);
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__2A49584C7578996E");

            entity.ToTable("Rol");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("PK__Usuario__B4ADFE39CFDF1AFC");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.NombreUsuario, "UQ__Usuario__6B0F5AE07F05D277").IsUnique();

            entity.Property(e => e.Cedula).ValueGeneratedNever();
            entity.Property(e => e.Activo).HasDefaultValueSql("((1))");
            entity.Property(e => e.Clave).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(50);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreUsuario).HasMaxLength(50);
            entity.Property(e => e.ResetearClave).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Usuario__IdRol__3D5E1FD2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
