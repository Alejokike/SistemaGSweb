namespace SistemaGS.DTO.Query
{
    public class InventarioQuery
    {
        //Propiedades
        public int IdItem { get; set; }
        public DateTime? FechaIni { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? filtro { get; set; }
        //Paginación
        public string? OrdenarPor { get; set; } = "";
        public bool Ascendente { get; set; } = true;
        public int Pagina { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
