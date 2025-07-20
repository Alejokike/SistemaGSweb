using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace SistemaGS.DTO
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public int? IdRol { get; set; }
        public string? Rol { get; set; }
        [Required(ErrorMessage = "Ingrese Nombre Completo")]
        public string NombreCompleto { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese Correo")]
        public string Correo { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese Nombre Usuario")]
        public string NombreUsuario { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese Contraseña")]
        public string Clave { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese Confirmar Contraseña")]
        public string ConfirmarClave { get; set; } = null!;
        public bool? ResetearClave { get; set; }
        public bool? Activo { get; set; }
    }
}
