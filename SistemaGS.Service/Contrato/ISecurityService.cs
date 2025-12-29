using SistemaGS.DTO;
using SistemaGS.DTO.AuthDTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;

namespace SistemaGS.Service.Contrato
{
    public interface ISecurityService
    {
        //Auditoría
        Task<bool> Registrar(RegistroDTO registro);
        Task<List<RegistroDTO>> Listar(RegistroQuery filtro);
        Task<RegistroDTO> Obtener(int id);

        //Seguridad
        Task<SesionDTO> Autorizacion(LoginDTO login);
        Task<SesionDTO> ObtenerById(int cedula);
        Task<AuthResponse> Refresh(string refreshtoken);
        Task Logout(string refreshtoken);
        Task<bool> InsertRefreshToken(RefreshToken refreshToken, int cedula);
    }
}
