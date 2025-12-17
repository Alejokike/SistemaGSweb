using SistemaGS.DTO.Query;
using SistemaGS.Model;

namespace SistemaGS.Repository.Contrato
{
    public interface IInventarioRepository : IGenericoRepository<Inventario>
    {
        Task<List<Item>> ListarInventario(ItemQuery filtro);
        Task<bool> Registrar(Inventario transaccion, Item item);
        Task<bool> Desbloquear(List<Inventario> movimientos, int IdAyuda);
    }
}
