using SistemaGS.Model;

namespace SistemaGS.Repository.Contrato
{
    public interface IPlanillaRepository : IGenericoRepository<Planilla>
    {
        Task<Planilla> Registrar(Planilla model);
    }
}
