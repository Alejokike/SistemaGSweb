namespace SistemaGS.Model;
public partial class Inventario
{
    public int IdTransaccion { get; set; }
    public string? TipoOperacion { get; set; }
    public int? Item { get; set; }
    public string? Unidad { get; set; }
    public decimal? Cantidad { get; set; }
    public string? Concepto { get; set; }
    public DateTime? Fecha { get; set; }
}
