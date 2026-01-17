using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SistemaGS.DTO.ModelDTO
{
    public class PersonaDTO
    {
        [Required(ErrorMessage = "Ingrese Cédula")]
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El número debe tener 7 u 8 dígitos.")]
        public int? Cedula { get; set; }

        [Required(ErrorMessage = "Ingrese Nombres")]
        [RegularExpression(@"^[\p{L} ]+$", ErrorMessage = "¿Por que tendrías eso de nombre? ¿¿¿???")]
        [MaxLength(60, ErrorMessage = "Máximo 60 caracteres")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese Apellidos")]
        [RegularExpression(@"^[\p{L} ]+$", ErrorMessage = "¿Por que tendrías eso de apellido? ¿¿¿???")]
        [MaxLength(60, ErrorMessage = "Máximo 60 caracteres")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese Fecha de Nacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Seleccione un Género")]
        [MaxLength(1, ErrorMessage = "Género inválido")]
        public string Genero { get; set; } = "M";

        [Required(ErrorMessage = "Ingrese Profesión")]
        [MaxLength(30, ErrorMessage = "Máximo 30 caracteres")]
        public string Profesion { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese Ocupación")]
        [MaxLength(30, ErrorMessage = "Máximo 30 caracteres")]
        public string Ocupacion { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese nombre del lugar de Trabajo")]
        [MaxLength(60, ErrorMessage = "Máximo 60 caracteres")]
        public string LugarTrabajo { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese Dirección de Trabajo")]
        [MaxLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public string DireccionTrabajo { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese Teléfono de Trabajo")]
        [RegularExpression(@"^0[24]\d{2}-\d{7}$", ErrorMessage = "Teléfono inválido")]
        [MaxLength(12, ErrorMessage = "Máximo 12 caracteres")]
        public string? TelefonoTrabajo { get; set; }

        [Required(ErrorMessage = "Ingrese Dirección de Habitación")]
        [MaxLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public string DireccionHabitacion { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese Teléfono de Trabajo")]
        [RegularExpression(@"^0[24]\d{2}-\d{7}$", ErrorMessage = "Teléfono inválido")]
        [MaxLength(12, ErrorMessage = "Máximo 12 caracteres")]
        public string TelefonoHabitacion { get; set; } = null!;
    }
    public class PersonaPersistent
    {
        public int Cedula { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public DateTime FechaNacimiento { get; set; } = DateTime.Now;
        public string Genero { get; set; } = "";
        public string Profesion { get; set; } = "";
        public string Ocupacion { get; set; } = "";
        public string LugarTrabajo { get; set; } = "";
        public string DireccionTrabajo { get; set; } = "";
        public string TelefonoTrabajo { get; set; } = "";
        public string DireccionHabitacion { get; set; } = "";
        public string TelefonoHabitacion { get; set; } = "";
    }
}