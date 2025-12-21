using Microsoft.EntityFrameworkCore;
using SistemaGS.Model;
using SistemaGS.DTO;
using SistemaGS.Repository.Contrato;
using SistemaGS.Service.Contrato;
using AutoMapper;
using SistemaGS.Util;
using SistemaGS.DTO.ModelDTO;

namespace SistemaGS.Service.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericoRepository<Usuario> _modelRepository;
        private readonly IUsuarioRepository _UsuarioRepository;
        private readonly IGenericoRepository<Rol> _RolRepository;
        
        private readonly IMapper _mapper;
        public UsuarioService(IGenericoRepository<Usuario> modelRepository ,IUsuarioRepository UsuarioRepository, IGenericoRepository<Rol> RolRepository,IMapper mapper)
        {
            _modelRepository = modelRepository;
            _UsuarioRepository = UsuarioRepository;
            _RolRepository = RolRepository;
            _mapper = mapper;
        }

        public async Task<SesionDTO> Autorizacion(LoginDTO Model)
        {
            try
            {
                if (await _modelRepository.Consultar(l => l.NombreUsuario == Model.NombreUsuario && l.Activo == true).AnyAsync())
                {
                    var fromDBmodel = await _modelRepository.Consultar(p => p.NombreUsuario == Model.NombreUsuario && p.Clave == Ferramentas.ConvertToSha256(Model.Clave) && p.Activo == true).FirstOrDefaultAsync();

                    if (fromDBmodel != null)
                    {
                        var sesion = _mapper.Map<SesionDTO>(fromDBmodel);
                        sesion.Rol = _mapper.Map<RolDTO>(await _RolRepository.Consultar(r => r.IdRol == fromDBmodel.IdRol).FirstAsync());
                        return sesion;
                    }
                    else throw new InvalidOperationException("Contraseña inválida");
                }
                else throw new TaskCanceledException("No se encontraron coincidencias");                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<UsuarioDTO> Crear(UsuarioDTO Model)
        {
            try
            {
                var DbUsuario = _mapper.Map<Usuario>(Model);
                var DbPersona = _mapper.Map<Persona>(Model.Persona);
                DbUsuario.Clave = Ferramentas.ConvertToSha256(DbUsuario.Clave);

                if (await _UsuarioRepository.Registrar(DbUsuario, DbPersona)) return await Obtener(Model.Cedula!.Value);
                else throw new TaskCanceledException("No se pudo crear");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<bool> Editar(UsuarioDTO Model)
        {
            try
            {
                var parseoU = _mapper.Map<Usuario>(Model);
                var parseoP = _mapper.Map<Persona>(Model.Persona);

                parseoU.Clave = Ferramentas.ConvertToSha256(Model.Clave);
                parseoU.ResetearClave = false;

                bool respuesta = await _UsuarioRepository.Editar(parseoU, parseoP);

                if (!respuesta) throw new TaskCanceledException("No se pudo editar");
                else return respuesta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var respuesta =  await _UsuarioRepository.Eliminar(id);
                if (!respuesta) throw new TaskCanceledException("No se pudo eliminar");
                else return respuesta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<List<UsuarioDTO>> Lista(int rol, string buscar)
        {
            try
            {
                var listaRepository = await _UsuarioRepository.Listar(rol, buscar);

                List<UsuarioDTO> lista = new List<UsuarioDTO>();

                foreach (var t in listaRepository)
                {
                    UsuarioDTO usuario = _mapper.Map<UsuarioDTO>(t.usuario);
                    usuario.Persona = _mapper.Map<PersonaDTO>(t.persona);
                    usuario.Rol = _mapper.Map<RolDTO>(t.rol);
                    lista.Add(usuario);
                }

                return lista;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<UsuarioDTO> Obtener(int id)
        {
            try
            {
                var response = await _UsuarioRepository.Obtener(id);

                var respuesta = _mapper.Map<UsuarioDTO>(response.usuario);
                respuesta.Rol = _mapper.Map<RolDTO>(response.rol);
                respuesta.Persona = _mapper.Map<PersonaDTO>(response.persona);
                return respuesta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
