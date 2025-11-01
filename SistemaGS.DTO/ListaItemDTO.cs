using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO
{
    public class ListaItemDTO
    {
        int IdLista {  get; set; }
        [Required(ErrorMessage = "Ingrese un item")]
        ItemDTO? ItemLista {  get; set; }
        [Required(ErrorMessage = "Ingrese una cantidad")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor no puede ser negativo")]
        decimal CantidadSolicitada { get; set; }
        [Required(ErrorMessage = "Ingrese una cantidad")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor no puede ser negativo")]
        decimal CantidadEntregada { get; set; } 
    }
}
