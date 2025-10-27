using SistemaGS.Model;

namespace SistemaGS.Repository.Contrato
{
    public interface IInventarioRepository : IGenericoRepository<Inventario>
    {
        Task<Inventario> Registrar(Inventario model);
        //Task<Ayuda> Desbloqeuar(List<Inventario> movimientos, Ayuda ayuda);
    }
}
