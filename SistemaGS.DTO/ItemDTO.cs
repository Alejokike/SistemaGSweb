using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO
{
    public class ItemDTO
    {
        public int IdItem { get; set; }
        [Required(ErrorMessage = "Ingrese Tipo Item")]
        public string? TipoItem { get; set; }
        [Required(ErrorMessage = "Ingrese Nombre Item")]
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese Cantidad Solicitada")]
        public int CantidadSolicitada { get; set; }
        [Required(ErrorMessage = "Ingrese Cantidad Entregada")]
        public int CantidadEntregada { get; set; }
        [Required(ErrorMessage = "Ingrese Monto Solicitado")]
        public decimal MontoSolicitado { get; set; }
        [Required(ErrorMessage = "Ingrese Monto Entregado")]
        public decimal MontoEntregado { get; set; }
    }
}
