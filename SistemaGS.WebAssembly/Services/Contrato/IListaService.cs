using SistemaGS.DTO;

namespace SistemaGS.WebAssembly.Services.Contrato
{
    public interface IListaService
    {
        //event Action MostrarItems;
        int CantidadItems();
        Task AgregarLista(ListaItemDTO item);
        Task EliminarLista(int idLista);
        Task<List<ListaItemDTO>> Listar();
        Task LimpiarLista();
    }
}
