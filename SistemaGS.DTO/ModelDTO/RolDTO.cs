using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace SistemaGS.DTO.ModelDTO
{
    public class RolDTO
    {
        [Range(1,3, ErrorMessage = "Ingrese un rol")]
        public int IdRol { get; set; }
        public string? Nombre { get; set; }
    }
}
