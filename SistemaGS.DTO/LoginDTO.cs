using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Ingrese nombre de usuario")]
        public string NombreUsuario { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese Contraseña")]
        public string Clave { get; set; } = null!;
    }
}
