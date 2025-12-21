using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO.ModelDTO
{
    public class ItemDTO
    {
        //[RegularExpression("^\\d+$\r\n", ErrorMessage = "Los códigos son netamente númericos")]
        [Display(Name = "ID")]
        public int IdItem { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese una categoría")]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; } = "";
        [Required(ErrorMessage = "Ingrese más información")]
        [Display(Name = "Descripción", AutoGenerateField = false)]
        public string Descripcion { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese una unidad")]
        public string Unidad { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese una cantidad")]
        [RegularExpression("^\\d+(,\\d{1,2})?$", ErrorMessage = "Formato inválido")]
        public decimal Cantidad { get; set; }
        [Display(AutoGenerateField = false)]
        public bool Activo { get; set; }
        [Display(Name = "Fecha de Creación", AutoGenerateField = false)]
        public DateTime FechaCreacion { get; set; }
    }
}
