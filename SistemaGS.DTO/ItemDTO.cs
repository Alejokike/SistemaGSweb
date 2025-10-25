using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO
{
    public class ItemDTO
    {
        public int IdItem { get; set; }
        [Required(ErrorMessage = "Ingrese Categoría")]
        public string? Categoria { get; set; }
        [Required(ErrorMessage = "Ingrese Nombre Item")]
        public string Nombre { get; set; } = null!;
    }
}
