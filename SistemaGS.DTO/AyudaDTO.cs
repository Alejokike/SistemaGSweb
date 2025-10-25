namespace SistemaGS.DTO
{
    public class AyudaDTO
    {
        public int IdPlanilla { get; set; }

        public int Solicitante { get; set; }

        public byte EdadSolicitante { get; set; }

        public int Beneficiario { get; set; }

        public byte EdadBeneficiario { get; set; }

        public int Funcionario { get; set; }

        public int Categoria { get; set; }
        public string? Descripcion { get; set; }

        public List<DetalleItemDTO>? ListaItems { get; set; }

        public DateTime? FechaSolicitud { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public bool? Activo { get; set; }
    }
}
