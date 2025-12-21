using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;

namespace SistemaGS.WebAssembly.Services.Contrato
{
    public interface IAuditoriaService
    {
        Task<ResponseDTO<List<RegistroDTO>>> Listar(RegistroQuery filtro);
        Task<ResponseDTO<RegistroDTO>> Obtener(int id);

    }
}
