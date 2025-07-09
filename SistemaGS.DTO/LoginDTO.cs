using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Ingrese Correo")]
        public string Correo { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese Contraseña")]
        public string Clave { get; set; } = null!;
    }
}
