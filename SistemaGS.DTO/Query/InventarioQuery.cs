namespace SistemaGS.DTO.Query
{
    public class InventarioQuery
    {
        public int IdItem { get; set; }
        public string? Movimiento { get; set; } = "";
        public string? Unidad { get; set; } = "";
        public DateTime? FechaIni { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
