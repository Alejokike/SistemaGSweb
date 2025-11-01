using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO
{
    public class AyudaDTO
    {
        public int IdAyuda { get; set; }
        [Required(ErrorMessage = "Ingrese Cédula solicitante")]
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El número debe tener 7 u 8 dígitos.")]
        public int Solicitante { get; set; }
        [Required(ErrorMessage = "Ingrese Cédula funcionario")]
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El número debe tener 7 u 8 dígitos.")]
        public int Funcionario { get; set; }
        [Required(ErrorMessage = "Seleccione una categoría")]
        public string? Categoria { get; set; }
        [Required(ErrorMessage = "Ingrese más información")]
        public string? Detalle { get; set; }
        public ListaItemDTO? ListaItems { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public DateTime? FechaEntrega { get; set; }
    }
}
