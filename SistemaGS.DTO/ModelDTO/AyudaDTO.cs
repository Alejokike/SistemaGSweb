using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO.ModelDTO
{
    public class AyudaDTO
    {
        public int IdAyuda { get; set; }
        public int Solicitante { get; set; }
        public int Funcionario { get; set; }
        [Required(ErrorMessage = "Seleccione una categoría")]
        public string? Categoria { get; set; }
        [Required(ErrorMessage = "Ingrese más información")]
        public string? Detalle { get; set; }
        public List<ListaItemDTO> ListaItems { get; set; } = null!;
        public string? Estado { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public DateTime? FechaEntrega { get; set; }
    }
}
