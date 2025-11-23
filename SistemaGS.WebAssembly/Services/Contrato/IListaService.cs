using SistemaGS.DTO.Responses;

namespace SistemaGS.WebAssembly.Services.Contrato
{
    public interface IListaService
    {
        //event Action MostrarItems;
        int CantidadItems();
        Task AgregarLista(ItemInventario item);
        Task EliminarLista(int id);
        Task<List<ItemInventario>> Listar();
        Task LimpiarLista();
    }
}
