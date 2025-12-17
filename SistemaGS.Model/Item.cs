namespace SistemaGS.Model;

public partial class Item
{
    public int IdItem { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Categoria { get; set; }

    public string Descripcion { get; set; } = null!;

    public string? Unidad { get; set; }

    public decimal? Cantidad { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public bool? Activo { get; set; }
}
