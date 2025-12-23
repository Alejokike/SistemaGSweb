using SistemaGS.DTO.AuthDTO;

namespace SistemaGS.Service.Contrato
{
    public interface ISecurityService
    {
        public bool InsertRefreshToken(RefreshToken refreshToken, int cedula);
    }
}
