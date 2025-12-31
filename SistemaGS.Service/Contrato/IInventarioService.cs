using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;

namespace SistemaGS.Service.Contrato
{
    public interface IInventarioService
    {
        Task<List<InventarioDTO>> Lista(InventarioQuery filtro);
        Task<InventarioDTO> Obtener(int IdTransaccion);
        Task<List<ItemDTO>> ListarInventario(ItemQuery filtro);
        Task<ItemDTO> ObtenerItem(int IdItem, string nombre);
        Task<InventarioDTO> Registrar(InventarioDTO Transaccion);
        Task<bool> Desbloquear(List<InventarioDTO> movimientos, int IdAyuda);
        Task<bool> Editar(ItemDTO model);
    }
}
