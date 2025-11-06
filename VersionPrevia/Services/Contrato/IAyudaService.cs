using SistemaGS.DTO;

namespace SistemaGS.WebAssembly.Services.Contrato
{
    public interface IAyudaService
    {
        event Action MostrarItems;

        int CantidadItems();
        Task AgregarItem(ItemDTO model);
        Task EliminarItem(int IdItem);
        Task<List<ItemDTO>> DevolverItems();
        Task Limpiar();
    }
}
