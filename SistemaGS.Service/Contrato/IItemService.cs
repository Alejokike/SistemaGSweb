using SistemaGS.DTO;

namespace SistemaGS.Service.Contrato
{
    public interface IItemService
    {
        Task<List<ItemDTO>> Lista(int tipo, string buscar);
        Task<ItemDTO> Obtener(int id);
        Task<ItemDTO> Crear(ItemDTO Model);
        Task<bool> Editar(ItemDTO Model, int Cedula);
        Task<bool> Eliminar(int id);
    }
}
