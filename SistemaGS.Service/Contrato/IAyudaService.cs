using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;

namespace SistemaGS.Service.Contrato
{
    public interface IAyudaService
    {
        Task<List<AyudaDTO>> Lista(AyudaQuery filtro);
        Task<AyudaDTO> Obtener(int idAyuda);
        Task<AyudaDTO> Crear(AyudaDTO Model);
        Task<bool> Editar(AyudaDTO Model);
        Task<bool> CambiarEstado(string estado, int idAyuda);
        Task<bool> Eliminar(int idAyuda);
    }
}
