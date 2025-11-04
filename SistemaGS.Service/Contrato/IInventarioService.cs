using SistemaGS.DTO.ModelDTO;

namespace SistemaGS.Service.Contrato
{
    public interface IInventarioService
    {
        Task<List<InventarioDTO>> Lista(int IdItem, string filtro, DateTime? FechaIni, DateTime? FechaFin);
        Task<List<ItemDTO>> ListarInventario(string filtro);
        Task<InventarioDTO> Obtener(int IdTransaccion);
        Task<InventarioDTO> Registrar(InventarioDTO Transaccion);
        Task<bool> Desbloquear(List<InventarioDTO> movimientos, int IdAyuda);
    }
}
