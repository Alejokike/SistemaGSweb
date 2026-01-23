namespace SistemaGS.Model;
public partial class Categoria
{
    public int IdCategoria { get; set; }
    public string TipoCategoria { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public bool? Activo { get; set; }
    public DateTime? FechaAccion { get; set; }
}
