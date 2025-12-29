using AutoMapper;
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
        private readonly IGenericoRepository<Registro> _auditoriaRepository;
        private readonly IGenericoRepository<Usuario> _usuarioRepository;
        private readonly IMapper _mapper;
        public SecurityService(IGenericoRepository<Usuario> usuarioRepository, IGenericoRepository<Registro> auditoriaRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _auditoriaRepository = auditoriaRepository;
            _mapper = mapper;
        }
        public async Task<bool> Registrar(RegistroDTO registro)
        {
            try
            {
                var responseDB = await _auditoriaRepository.Crear(_mapper.Map<Registro>(registro));
                return responseDB is not null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<List<RegistroDTO>> Listar(RegistroQuery filtro)
        {
            try
            {
                var registros = _auditoriaRepository.Consultar();

                var lista = from r in registros
                            where
                            (r.FechaAccion >= filtro.FechaIni && r.FechaAccion <= filtro.FechaFin) &&
                            (string.IsNullOrEmpty(filtro.Accion) || r.Accion == filtro.Accion) &&
                            (filtro.UsuarioResponsable == 0 || filtro.UsuarioResponsable == r.UsuarioResponsable)
                            select r;
                return _mapper.Map<List<RegistroDTO>>(await lista.ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<RegistroDTO> Obtener(int id)
        {
            try
            {
                var responseDB = await _auditoriaRepository.Consultar(r => r.IdRegistro == id).FirstOrDefaultAsync();
                if (responseDB is not null) return _mapper.Map<RegistroDTO>(responseDB);
                else throw new TaskCanceledException("No existe ese registro");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
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
        public Task Logout(string refreshtoken)
        {
            throw new NotImplementedException();
        }
        public Task<AuthResponse> Refresh(string refreshtoken)
        {
            throw new NotImplementedException();
        }
    }
}
