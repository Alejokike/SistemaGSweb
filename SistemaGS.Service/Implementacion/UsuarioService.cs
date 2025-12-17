using Microsoft.EntityFrameworkCore;
using SistemaGS.Model;
using SistemaGS.DTO;
using SistemaGS.Repository.Contrato;
using SistemaGS.Service.Contrato;
using AutoMapper;
using SistemaGS.Util;
using SistemaGS.DTO.ModelDTO;
using System.ComponentModel;
using System.Threading.Tasks.Dataflow;

namespace SistemaGS.Service.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericoRepository<Usuario> _modelRepository;
        private readonly IUsuarioRepository _UsuarioRepository;
        private readonly IGenericoRepository<Rol> _RolRepository;
        private readonly IGenericoRepository<Persona> _PersonaRepository;
        
        private readonly IMapper _mapper;
        public UsuarioService(IGenericoRepository<Usuario> modelRepository ,IUsuarioRepository UsuarioRepository, IGenericoRepository<Rol> RolRepository, IGenericoRepository<Persona> PersonaRepository,IMapper mapper)
        {
            _modelRepository = modelRepository;
            _UsuarioRepository = UsuarioRepository;
            _RolRepository = RolRepository;
            _PersonaRepository = PersonaRepository;
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

                if (Model.Cedula == null) throw new TaskCanceledException("No se pudo crear");
                if (await _UsuarioRepository.Registrar(DbUsuario, DbPersona)) return await Obtener(Model.Cedula.Value);
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
                /*
                IQueryable<Usuario>? consulta;

                if(rol != 0)
                {
                    consulta = _modelRepository.Consultar(p => p.IdRol == Rol && string.Concat(p.Cedula.ToString(), p.NombreUsuario.ToLower(), p.Correo.ToLower()).Contains(buscar.ToLower()));
                }
                else
                {
                    consulta = _modelRepository.Consultar(p => string.Concat(p.Cedula.ToString(), p.NombreUsuario.ToLower(), p.Correo.ToLower()).Contains(buscar.ToLower()));
                }
                */
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
                var (usuario, persona, rol) = await _UsuarioRepository.Obtener(id);

                if (usuario != null && persona != null && rol != null)
                {
                    var respuesta = _mapper.Map<UsuarioDTO>(usuario);
                    respuesta.Rol = _mapper.Map<RolDTO>(rol);
                    respuesta.Persona = _mapper.Map<PersonaDTO>(persona);
                    return respuesta;
                }
                else throw new TaskCanceledException("");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
