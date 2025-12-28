using SistemaGS.DTO.AuthDTO;
using SistemaGS.Repository.Contrato;

namespace SistemaGS.Repository.Implementacion
{
    public class SecurityRepository : ISecurityRepository
    {
        public SecurityRepository()
        {
            
        }
        public bool DisableUserToken(string token)
        {
            throw new NotImplementedException();
        }

        public bool DisableUserTokenByCedula(int Cedula)
        {
            throw new NotImplementedException();
        }

        public int FindUserByToken(string token)
        {
            throw new NotImplementedException();
        }

        public bool InsertRefreshToken(RefreshToken refreshToken, int Cedula)
        {
            throw new NotImplementedException();
        }

        public bool IsRefreshTokenValid(string token)
        {
            throw new NotImplementedException();
        }
    }
}
