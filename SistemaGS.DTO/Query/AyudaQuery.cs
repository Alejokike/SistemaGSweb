namespace SistemaGS.DTO.Query
{
    public class AyudaQuery
    {
        //propiedades filtro
        public string categoria { get; set; } = "";
        public string buscar { get; set; } = "";
        public int solicitante { get; set; } = 0;
        public int funcionario { get; set; } = 0;
        public DateTime? FechaIni { get; set; } = null;
        public DateTime? FechaFin { get; set; } = null;
        //propiedades paginación
        public string OrdenarPor { get; set; } = "";
        public bool Ascendente { get; set; } = false;
        public int Pagina { get; set; } = 1;
        public int PageSize {  get; set; } = 20;
    }
}
