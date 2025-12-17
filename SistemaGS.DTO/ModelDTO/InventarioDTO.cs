using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO.ModelDTO
{
    public class InventarioDTO
    {
        [Display(Name = "ID Transacción")]
        public int IdTransaccion { get; set; }
        [Required(ErrorMessage = "Seleccione un tipo de operación")]
        [Display(Name = "Tipo de Operación")]
        public string? TipoOperacion { get; set; }
        public ItemDTO Item { get; set; } = new ItemDTO();
        public string? Unidad { get; set; }
        [Required(ErrorMessage = "Ingrese una cantidad")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor no puede ser negativo")]
        [RegularExpression("^\\d+(,\\d{1,2})?$", ErrorMessage = "Formato inválido")]
        public decimal Cantidad { get; set; }
        [Required(ErrorMessage = "Ingrese concepto")]
        public string? Concepto { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Today;
    }
}
