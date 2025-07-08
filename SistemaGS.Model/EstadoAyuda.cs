namespace SistemaGS.Model;

public partial class EstadoAyuda
{
    public int IdEstado { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<Planilla> Planillas { get; set; } = new List<Planilla>();
}
