namespace SistemaGS.DTO.Query
{
    public class RegistroQuery
    {
        public string Accion { get; set; } = "";
        public int UsuarioResponsable { get; set; } = 0;
        public DateTime? FechaIni {  get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
