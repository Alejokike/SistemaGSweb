namespace SistemaGS.DTO.Query
{
    public class AyudaQuery
    {
        public string? categoria { get; set; } = "";
        public int? solicitante { get; set; } = null;
        public int? funcionario { get; set; } = null;
        public string? DataSoli { get; set; } = null; 
        public string? DataFunci { get; set; } = null; 
        public string? Estado { get; set; } = "";
        public DateTime? FechaIni { get; set; } = null;
        public DateTime? FechaFin { get; set; } = null;
    }
}
