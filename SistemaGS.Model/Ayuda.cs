namespace SistemaGS.Model;
public partial class Ayuda
{
    public int IdAyuda { get; set; }
    public int? Solicitante { get; set; }
    public int? Funcionario { get; set; }
    public string? Categoria { get; set; }
    public string? Detalle { get; set; }
    public string? ListaItems { get; set; }
    public string? Estado { get; set; }
    public DateTime? FechaSolicitud { get; set; }
    public DateTime? FechaEntrega { get; set; }
    public virtual Persona FuncionarioNavigation { get; set; } = null!;
    public virtual Persona SolicitanteNavigation { get; set; } = null!;
}
