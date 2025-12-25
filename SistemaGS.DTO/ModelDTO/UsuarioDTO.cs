using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO.ModelDTO
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "Ingrese Cédula")]
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El número debe tener 7 u 8 dígitos.")]
        public int? Cedula { get; set; }
        public RolDTO Rol { get; set; } = null!;
        public PersonaDTO Persona { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese nombre de usuario")]
        [RegularExpression(@"^[A-Za-z0-9_]+$", ErrorMessage = "El usuario no puede tener espacios ni caracteres especiales.")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string NombreUsuario { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese correo electrónico")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Ingrese un correo electrónico válido")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese contraseña")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "La clave debe tener al menos 8 caracteres, incluyendo mayúsculas, minúsculas, número y símbolo")]
        public string Clave { get; set; } = null!;

        [Required(ErrorMessage = "Confirme su contraseña")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "La clave debe tener al menos 8 caracteres, incluyendo mayúsculas, minúsculas, número y símbolo")]
        public string ConfirmarClave { get; set; } = null!;
        public bool Activo { get; set; }
        public bool ResetearClave { get; set; } = true;
        public DateTime FechaCreacion { get; set; }
    }
    public class UsuarioPersistent
    {
        public int? Cedula { get; set; }
        public RolDTO Rol { get; set; } = null!;
        public PersonaDTO Persona { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Clave { get; set; } = null!;
        public string ConfirmarClave { get; set; } = null!;
        public bool Activo { get; set; }
        public bool ResetearClave { get; set; } = true;
        public DateTime FechaCreacion { get; set; }
    }
}
