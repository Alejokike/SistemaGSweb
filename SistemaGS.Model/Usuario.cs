namespace SistemaGS.Model;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int? IdRol { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string NombreUsuario { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public bool? ResetearClave { get; set; }

    public bool? Activo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }

    public virtual ICollection<Registro> Registros { get; set; } = new List<Registro>();
}
