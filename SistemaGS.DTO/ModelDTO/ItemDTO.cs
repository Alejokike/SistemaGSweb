using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO.ModelDTO
{
    public class ItemDTO
    {
        public int IdItem { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese una categoría")]
        public string Categoria { get; set; } = "";
        [Required(ErrorMessage = "Ingrese más información")]
        public string Descripcion { get; set; } = null!;
        public string Unidad { get; set; } = "EU";
    }
}
