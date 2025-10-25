using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO
{
    public class DetalleItemDTO
    {
        public int IDlista { get; set; }
        [Required(ErrorMessage = "Seleccione un Item")]
        ItemDTO? ItemEntrega { get; set; }
        public int CantidadSolicitada { get; set; }
        [Required(ErrorMessage = "Ingrese Cantidad Entregada")]
        public int CantidadEntregada { get; set; }
        [Required(ErrorMessage = "Ingrese Unidad de medida")]
        public string? Unidad { get; set; }
        [Required(ErrorMessage = "Ingrese Nombre de persona encargada")]
        public string? PersonaEncargada { get; set; }
        [Required(ErrorMessage = "Ingrese Nombre de la institución")]
        public string? InstitucionEncargada { get; set; }
    }
}
