using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.Model;

namespace SistemaGS.Repository.Contrato
{
    public interface IAyudaRepository
    {
        public Task<bool> MasiveAttack(List<Ayuda> ayudas);
        public Task<List<Ayuda>> Listar(AyudaQuery filtro);
    }
}
