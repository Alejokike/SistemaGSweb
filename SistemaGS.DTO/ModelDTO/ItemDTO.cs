using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO.ModelDTO
{
    public class ItemDTO
    {
        public int IdItem { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "Seleccione la categoría")]
        public string? Categoria { get; set; }
        [Required(ErrorMessage = "Ingrese maás información")]
        public string Descripcion { get; set; } = null!;
        [Required(ErrorMessage = "Seleccione una unidad")]
        public string? Unidad { get; set; }
    }
}
