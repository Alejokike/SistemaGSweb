using SistemaGS.DTO;
using SistemaGS.DTO.AuthDTO;
using SistemaGS.DTO.ModelDTO;

namespace SistemaGS.Service.Contrato
{
    public interface ISecurityService
    {
        Task<bool> Registrar(RegistroDTO registro);
        Task<SesionDTO> Autorizacion(LoginDTO login);
        Task<AuthResponse> Refresh(string refreshtoken);
        Task Logout(string refreshtoken);
        Task<bool> InsertRefreshToken(RefreshToken refreshToken, int cedula);
    }
}
