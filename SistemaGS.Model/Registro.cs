namespace SistemaGS.Model;

public partial class Registro
{
    public int IdRegistro { get; set; }

    public string? TablaAfectada { get; set; }

    public int? IdRegistroAfectado { get; set; }

    public string? Accion { get; set; }

    public int? UsuarioResponsable { get; set; }

    public DateTime? FechaAccion { get; set; }

    public string? DetalleJson { get; set; }

    public virtual Usuario? UsuarioResponsableNavigation { get; set; }
}
