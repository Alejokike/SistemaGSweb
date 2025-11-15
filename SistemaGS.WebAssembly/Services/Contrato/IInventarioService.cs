using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.DTO.Responses;
using System.Text.Json;

namespace SistemaGS.WebAssembly.Services.Contrato
{
    public interface IInventarioService
    {
        Task<ResponseDTO<List<InventarioDTO>>> Lista(int IdItem, DateTime? FechaIni = null, DateTime? FechaFin = null, string filtro = "NA");
        Task<ResponseDTO<InventarioRespuesta>> ListarInventario(ItemQuery filtro);
        Task<ResponseDTO<InventarioDTO>> Obtener(int IdTransaccion);
        Task<ResponseDTO<InventarioDTO>> Registrar(InventarioDTO Transaccion);
        Task<ResponseDTO<bool>> Desbloquear(List<InventarioDTO> movimientos, int idAyuda);
    }
}
