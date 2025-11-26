using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Responses;

namespace SistemaGS.WebAssembly.Services.Contrato
{
    public interface IListaInventarioService
    {
        int CantidadItems();
        Task Registrar(InventarioDTO mov);
        Task Desbloquear(List<InventarioDTO> listaMovimientos, int idAyuda);
        Task<List<InventarioDTO>> ListarMovimientos();
        Task<List<ItemInventario>> ListarInventario();
    }
}
