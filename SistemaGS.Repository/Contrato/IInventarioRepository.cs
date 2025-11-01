using SistemaGS.Model;

namespace SistemaGS.Repository.Contrato
{
    public interface IInventarioRepository : IGenericoRepository<Inventario>
    {
        Task<bool> Registrar(Inventario transaccion, Item item);
        Task<bool> Desbloquear(List<Inventario> movimientos, Ayuda ayudaModificada);
    }
}
