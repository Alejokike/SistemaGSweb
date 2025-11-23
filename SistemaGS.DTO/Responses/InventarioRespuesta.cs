namespace SistemaGS.DTO.Responses
{
    public class ItemInventario
    {
        public int IdItem { get; set; }
        public string Nombre { get; set; } = null!;
        public string Categoria { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Unidad { get; set; } = null!;
        public decimal Cantidad { get; set; }
    }
    public class InventarioRespuesta
    {
        public string contenido { get; set; } = "";
        public int contador { get; set; }
    }
}
