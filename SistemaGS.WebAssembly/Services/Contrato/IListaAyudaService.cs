using SistemaGS.DTO.ModelDTO;

namespace SistemaGS.WebAssembly.Services.Contrato
{
    public interface IListaAyudaService
    {
        int CantidadItems();
        Task AgregarLista(AyudaDTO item);
        Task EliminarLista(int idLista);
        Task<List<AyudaDTO>> Listar();
        Task LimpiarLista();
    }
}
