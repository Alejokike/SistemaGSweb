using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO.ModelDTO
{
    public class RegistroDTO
    {
        [Display(Name = "ID Registro")]
        public int IdRegistro { get; set; }
        [Display(Name = "Tabla Afectada")]
        public string TablaAfectada { get; set; } = "";
        [Display(Name = "Registro Afectado")]
        public int IdRegistroAfectado { get; set; }
        [Display(Name = "Acción")]
        public string Accion { get; set; } = "";
        [Display(Name = "Usuario Responsable")]
        public int UsuarioResponsable { get; set; }
        [Display(AutoGenerateField = false)]

        public string Detalle { get; set; } = "";
        public DateTime FechaAccion { get; set; }
    }
}
