namespace SistemaGS.Model;

public partial class Planilla
{
    public int IdPlanilla { get; set; }

    public int Solicitante { get; set; }

    public byte EdadSolicitante { get; set; }

    public string TipoAyuda { get; set; } = null!;

    public string DescripcionAyuda { get; set; } = null!;

    public string? DescripcionJson { get; set; }

    public int Beneficiario { get; set; }

    public byte EdadBeneficiario { get; set; }

    public string Observaciones { get; set; } = null!;

    public int Funcionario { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public int? IdEstado { get; set; }

    public bool? Activo { get; set; }

    public virtual Persona BeneficiarioNavigation { get; set; } = null!;

    public virtual Persona FuncionarioNavigation { get; set; } = null!;

    public virtual EstadoAyuda? IdEstadoNavigation { get; set; }

    public virtual ICollection<ListaItem> ListaItems { get; set; } = new List<ListaItem>();

    public virtual Persona SolicitanteNavigation { get; set; } = null!;
}
