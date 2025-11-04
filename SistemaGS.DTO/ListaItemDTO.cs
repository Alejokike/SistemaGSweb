using SistemaGS.DTO.ModelDTO;
using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO
{
    public class ListaItemDTO
    {
        public int IdLista {  get; set; }
        [Required(ErrorMessage = "Ingrese un item")]
        public ItemDTO? ItemLista {  get; set; }
        [Required(ErrorMessage = "Ingrese una cantidad")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor no puede ser negativo")]
        public decimal CantidadSolicitada { get; set; }
        [Required(ErrorMessage = "Ingrese una cantidad")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor no puede ser negativo")]
        public decimal CantidadEntregada { get; set; } 
    }
}
