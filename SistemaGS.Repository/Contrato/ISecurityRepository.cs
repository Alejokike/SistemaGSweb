using SistemaGS.DTO.AuthDTO;

namespace SistemaGS.Repository.Contrato
{
    public interface ISecurityRepository
    {
        public bool InsertRefreshToken(RefreshToken refreshToken, int Cedula);
        public bool DisableUserTokenByCedula(int Cedula);
        public bool DisableUserToken(string token);
        public bool IsRefreshTokenValid(string token);
        public int FindUserByToken(string token);
    }
}
