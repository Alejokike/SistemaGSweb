using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO.ModelDTO
{
    public class CategoriaDTO
    {
        public CategoriaDTO()
        {
            
        }
        public CategoriaDTO(CategoriaDTO categoria)
        {
            IdCategoria = categoria.IdCategoria;
            TipoCategoria = categoria.TipoCategoria;
            Nombre = categoria.Nombre;
            Activo = categoria.Activo;
            FechaAccion = categoria.FechaAccion;
        }
        public int IdCategoria { get; set; }
        [Required(ErrorMessage = "Ingrese un tipo de categoría")]
        public string TipoCategoria { get; set; } = "";
        [Required(ErrorMessage = "Ingrese un nombre a la categoría")]
        [StringLength(50,ErrorMessage = "El nombre no puede tener mas de 50 caracteres")]
        public string Nombre { get; set; } = "";
        [Required(ErrorMessage = "Debe ser activo o inactivo")]
        public bool Activo { get; set; }
        public DateTime? FechaAccion { get; set; }
    }
}
