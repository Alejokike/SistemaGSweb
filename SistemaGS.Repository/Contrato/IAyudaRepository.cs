using SistemaGS.DTO.Query;
using SistemaGS.Model;

namespace SistemaGS.Repository.Contrato
{
    public interface IAyudaRepository
    {
        public Task<List<Ayuda>> Listar(AyudaQuery filtro);
    }
}
