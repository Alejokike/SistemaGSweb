using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using SistemaGS.DTO;
using SistemaGS.DTO.AuthDTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.Model;
using SistemaGS.Repository.Contrato;
using SistemaGS.Service.Contrato;
using SistemaGS.Util;

namespace SistemaGS.Service.Implementacion
{
    public class SecurityService : ISecurityService
    {
        //private readonly ISecurityRepository _securityRepository;
        private readonly IGenericoRepository<Usuario> _usuarioRepository;
        private readonly IMapper _mapper;
        public SecurityService(IGenericoRepository<Usuario> usuarioRepository, /*ISecurityRepository securityRepository,*/ IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            //_securityRepository = securityRepository;
            _mapper = mapper;
        }
        public async Task<SesionDTO> Autorizacion(LoginDTO login)
        {
            try
            {
                var responseBD = await _usuarioRepository.Consultar(u => u.NombreUsuario == login.NombreUsuario).FirstOrDefaultAsync();

                if (responseBD != null)
                {
                    if (Ferramentas.ConvertToSha256(login.Clave + responseBD.Cedula) == responseBD.Clave) return _mapper.Map<SesionDTO>(responseBD);
                    else throw new TaskCanceledException("Contraseña incorrecta");
                }
                else throw new TaskCanceledException("Usuario no encontrado");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<SesionDTO> ObtenerById(int cedula)
        {
            try
            {
                var responseBD = await _usuarioRepository.Consultar(u => u.Cedula == cedula).FirstOrDefaultAsync();

                if (responseBD != null) return _mapper.Map<SesionDTO>(responseBD);
                else throw new TaskCanceledException("Usuario no encontrado");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public Task<bool> InsertRefreshToken(RefreshToken refreshToken, int cedula)
        {
            throw new NotImplementedException();
        }

        public Task<List<RegistroDTO>> Listar(RegistroQuery filtro)
        {
            throw new NotImplementedException();
        }

        public Task Logout(string refreshtoken)
        {
            throw new NotImplementedException();
        }

        

        public Task<AuthResponse> Refresh(string refreshtoken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Registrar(RegistroDTO registro)
        {
            throw new NotImplementedException();
        }
    }
}
