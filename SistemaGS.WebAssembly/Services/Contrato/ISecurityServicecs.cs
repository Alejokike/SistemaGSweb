using SistemaGS.DTO;
using SistemaGS.DTO.AuthDTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;

namespace SistemaGS.WebAssembly.Services.Contrato
{
    public interface ISecurityService
    {
        //Auditoria
        Task<ResponseDTO<List<RegistroDTO>>> Listar(RegistroQuery filtro);
        Task<ResponseDTO<RegistroDTO>> Obtener(int id);
        //Security
        Task<ResponseDTO<SesionDTO>> Autorizar();
        Task<ResponseDTO<AuthResponse>> Refresh();
        Task Logout();
    }
}
