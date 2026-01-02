using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO.ModelDTO
{
    public class ItemDTO
    {
        public ItemDTO()
        {
        }
        public ItemDTO(ItemDTO item)
        {
            this.IdItem = item.IdItem;
            this.Nombre = item.Nombre;
            this.Categoria = item.Categoria;
            this.Descripcion = item.Descripcion;
            this.Unidad = item.Unidad;
            this.Cantidad = item.Cantidad;
            this.Activo = item.Activo;
            this.FechaCreacion = item.FechaCreacion;
        }
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
        //[Required(ErrorMessage = "Ingrese una cantidad")]
        [Display(Name = "Cantidad", AutoGenerateField = false)]
        //[RegularExpression(@"^\d+([,]\d{1,2})?$", ErrorMessage = "Formato inválido")]
        public decimal Cantidad { get; set; }
        [Display(AutoGenerateField = false, Description = "Admin")]
        public bool Activo { get; set; } = true;
        [Display(Name = "Fecha de Creación", AutoGenerateField = false, Description = "Admin")]
        public DateTime FechaCreacion { get; set; }
    }
}
