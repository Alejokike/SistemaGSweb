using SistemaGS.DTO.ModelDTO;
using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO
{
    public class ListaItemDTO
    {
        [Display(Name = "ID")]
        public int IdLista {  get; set; }
        [Required(ErrorMessage = "Ingrese un item")]
        public ItemDTO ItemLista {  get; set; } = null!;
        [Required(ErrorMessage = "Ingrese una cantidad")]
        public ItemDTO? ItemNoValidado { get; set; }
        [Required(ErrorMessage = "Ingrese una cantidad")]
        public decimal CantidadSolicitada { get; set; }
        [Required(ErrorMessage = "Ingrese una cantidad")]
        //[Range(0, double.MaxValue, ErrorMessage = "El valor no puede ser negativo")]
        public decimal CantidadEntregada { get; set; } 
    }
}
