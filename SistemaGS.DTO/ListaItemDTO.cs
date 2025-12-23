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
        //[Range(0.01, 9999999, ErrorMessage = "El valor no puede ser 0 ni mayor a 9.999.999")]
        [RegularExpression(@"^\d+([.,]\d{1,2})?$", ErrorMessage = "Formato inválido")]
        public decimal CantidadSolicitada { get; set; }
        [Required(ErrorMessage = "Ingrese una cantidad")]
        //[Range(0.01, 9999999, ErrorMessage = "El valor no puede ser 0 ni mayor a 9.999.999")]
        [RegularExpression(@"^\d+([.,]\d{1,2})?$", ErrorMessage = "Formato inválido")]
        public decimal CantidadEntregada { get; set; } 
    }
}
