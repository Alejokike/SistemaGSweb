namespace SistemaGS.Model;

public partial class Usuario
{
    public int Cedula { get; set; }

    public int? IdRol { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public bool? Activo { get; set; }

    public bool? ResetearClave { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }
}
