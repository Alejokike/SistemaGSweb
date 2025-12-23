using SistemaGS.DTO.AuthDTO;
using SistemaGS.DTO.ModelDTO;
using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO
{
    public class SesionDTO
    {
        [Required(ErrorMessage = "Ingrese cédula")]
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El número debe tener 7 u 8 dígitos.")]
        public int Cedula { get; set; }
        [Required(ErrorMessage = "Ingrese correo electrónico")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Ingrese un correo electrónico válido")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Correo { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public RolDTO Rol { get; set; } = null!;
        public AuthResponse AuthResponse { get; set; } = new AuthResponse();
    }
}
