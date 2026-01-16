using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO;
using SistemaGS.DTO.Query;

namespace SistemaGS.WebAssembly.Services.Contrato
{
    public interface IAyudaService
    {
        Task<ResponseDTO<List<AyudaDTO>>> ListarCerradas(AyudaQuery filtro);
        Task<ResponseDTO<List<AyudaDTO>>> Listar(AyudaQuery filtro);
        Task<ResponseDTO<AyudaDTO>> Obtener(int idAyuda);
        Task<ResponseDTO<AyudaDTO>> Crear(AyudaDTO ayuda);
        Task<ResponseDTO<bool>> Editar(AyudaDTO ayuda);
        Task<ResponseDTO<bool>> Eliminar(int idAyuda);
        Task<ResponseDTO<DashboardDTO>> Dashboard();
        Task<ResponseDTO<byte[]>> Imprimir(int idAyuda, int option);
        Task<ResponseDTO<byte[]>> ImprimirReporte(AyudaQuery filtro);
    }
}
