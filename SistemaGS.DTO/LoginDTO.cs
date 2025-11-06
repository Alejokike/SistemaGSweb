using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Ingrese nombre de usuario")]
        public string NombreUsuario { get; set; } = null!;
        public int Cedula { get; set; }
        [Required(ErrorMessage = "Ingrese correo electrónico")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Ingrese un correo electrónico válido")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Correo { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese Contraseña")]
        public string Clave { get; set; } = null!;
    }
}
