using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.DTO.Responses;

namespace SistemaGS.WebAssembly.Services.Contrato
{
    public interface IInventarioService
    {
        Task<ResponseDTO<List<InventarioDTO>>> Lista(InventarioQuery filtro);
        Task<ResponseDTO<InventarioDTO>> Obtener(int IdTransaccion);
        Task<ResponseDTO<InventarioRespuesta>> ListarInventario(ItemQuery filtro);
        Task<ResponseDTO<ItemDTO>> ObtenerItem(int IdItem, string nombre = "NA");
        Task<ResponseDTO<InventarioDTO>> Registrar(InventarioDTO Transaccion);
        Task<ResponseDTO<bool>> Desbloquear(List<InventarioDTO> movimientos, int idAyuda);
    }
}
