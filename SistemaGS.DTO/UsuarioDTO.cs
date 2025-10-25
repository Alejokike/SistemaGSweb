using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "Seleccione un rol")]
        public int? IdRol { get; set; }
        public string? Rol { get; set; }
        [Required(ErrorMessage = "Ingrese Nombre Completo")]
        [RegularExpression(@"^[\p{L} ]+$", ErrorMessage = "¿Por que tendrías eso de nombre? ¿¿¿???")]
        public string NombreCompleto { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese Correo")]
        [RegularExpression(@"^^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Correo no válido")]
        public string Correo { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese Cédula")]
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El número debe tener 7 u 8 dígitos.")]
        public int Cedula { get; set; }
        public PersonaDTO Persona { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese Nombre Usuario")]
        public string NombreUsuario { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese Contraseña")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "La clave debe tener al menos 8 caracteres, incluyendo mayúsculas, minúsculas, número y símbolo")]
        public string Clave { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese Confirmar Contraseña")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "La clave debe tener al menos 8 caracteres, incluyendo mayúsculas, minúsculas, número y símbolo")]
        public string ConfirmarClave { get; set; } = null!;
        public bool ResetearClave { get; set; }
        public bool Activo { get; set; }
    }
}
