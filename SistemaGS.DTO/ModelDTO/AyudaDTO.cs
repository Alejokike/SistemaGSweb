using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO.ModelDTO
{
    public class AyudaDTO
    {
        [Display(Name = "ID")]
        public int IdAyuda { get; set; }
        public int Solicitante { get; set; }
        public int Funcionario { get; set; }
        [Required(ErrorMessage = "Seleccione una categoría")]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese más información")]
        public Dictionary<string, string> Detalle { get; set; } = new()
        {
            ["Solicitud"] = "",
            ["Observaciones"] = "",
            ["Aprobado"] = "",
            ["Rechazado"] = ""
        };
        public List<ListaItemDTO> ListaItems { get; set; } = null!;
        public string Estado { get; set; } = null!;
        [Display(Name = "Fecha de Solicitud")]
        public DateTime FechaSolicitud { get; set; }
        [Display(Name = "Fecha de Resolución")]
        public DateTime? FechaEntrega { get; set; }
    }
}
