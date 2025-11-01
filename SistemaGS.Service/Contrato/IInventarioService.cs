using SistemaGS.DTO;

namespace SistemaGS.Service.Contrato
{
    public interface IInventarioService
    {
        Task<List<InventarioDTO>> Lista(int IdItem, string filtro, DateTime? FechaIni, DateTime? FechaFin);
        Task<InventarioDTO> Obtener(int IdTransaccion);
        Task<InventarioDTO> Registrar(InventarioDTO Transaccion);
        Task<bool> Desbloquear(List<InventarioDTO> movimientos, AyudaDTO ayuda);
    }
}
