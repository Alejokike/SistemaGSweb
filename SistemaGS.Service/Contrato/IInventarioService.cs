using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.DTO.Responses;

namespace SistemaGS.Service.Contrato
{
    public interface IInventarioService
    {
        Task<List<InventarioDTO>> Lista(InventarioQuery filtro);
        Task<InventarioDTO> Obtener(int IdTransaccion);
        Task<(string, int)> ListarInventario(ItemQuery filtro);
        Task<ItemDTO> ObtenerItem(int IdItem);
        Task<InventarioDTO> Registrar(InventarioDTO Transaccion);
        Task<bool> Desbloquear(List<InventarioDTO> movimientos, int IdAyuda);
    }
}
