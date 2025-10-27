namespace SistemaGS.DTO
{
    public class SesionDTO
    {
        public int Cedula { get; set; }
        public string Correo { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public RolDTO Rol { get; set; } = null!;
    }
}
