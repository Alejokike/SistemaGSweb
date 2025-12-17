namespace SistemaGS.DTO.Query
{
    public class ItemQuery
    {
        public int ID { get; set; } = 0;
        public string? Nombre { get; set; } = "";
        public string? Categoria { get; set; } = "";
        public string? Unidad { get; set; } = "";
        public bool? Activo { get; set; }
        public DateTime? FechaIni { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
