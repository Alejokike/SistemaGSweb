namespace SistemaGS.DTO.Query
{
    public class ItemQuery
    {
        //propiedades filtro
        public string? Nombre { get; set; }
        public string? Categoria { get; set; }
        public string? buscar { get; set; }
        public string? Unidad { get; set; }
        //propiedades paginación
        public string OrdenarPor { get; set; } = "";
        public bool Ascendente { get; set; } = false;
        public int Pagina { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
