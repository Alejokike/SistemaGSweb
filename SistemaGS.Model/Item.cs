namespace SistemaGS.Model;

public partial class Item
{
    public int IdItem { get; set; }

    public string? TipoItem { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<ListaItem> ListaItems { get; set; } = new List<ListaItem>();
}
