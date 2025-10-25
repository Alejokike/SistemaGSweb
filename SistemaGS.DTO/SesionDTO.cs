namespace SistemaGS.DTO
{
    public class SesionDTO
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public RolDTO Rol { get; set; }
        public PersonaDTO Persona { get; set; }
    }
}
