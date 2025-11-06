using SistemaGS.Model;

namespace SistemaGS.Repository.Contrato
{
    public interface IAyudaRepository
    {
        public Task<List<Ayuda>> Listar(string filtro);
        public Task<bool> CambiarEstado(string estado, int idAyuda);
    }
}
