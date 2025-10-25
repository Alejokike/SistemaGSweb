using SistemaGS.Model;

namespace SistemaGS.Repository.Contrato
{
    public interface IPlanillaRepository : IGenericoRepository<Ayuda>
    {
        Task<Ayuda> Registrar(Ayuda model);
    }
}
