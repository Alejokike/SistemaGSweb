using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Responses;

namespace SistemaGS.WebAssembly.Services.Contrato
{
    public interface IListaService
    {
        //event Action MostrarItems;
        Task CargarLista(List<UsuarioDTO> lista);
        int CantidadItems();
        Task AgregarLista(UsuarioDTO item);
        Task EliminarLista(int id);
        Task<List<UsuarioDTO>> Listar();
        Task LimpiarLista();
    }
}
