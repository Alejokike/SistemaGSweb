using Microsoft.EntityFrameworkCore;
using SistemaGS.Model;
using SistemaGS.DTO;
using SistemaGS.Repository.Contrato;
using SistemaGS.Service.Contrato;
using AutoMapper;
using SistemaGS.Util;

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
                if (await _modelRepository.Consultar(l => l.NombreUsuario == Model.NombreUsuario).AnyAsync())
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
                throw ex;
            }
        }

        public async Task<UsuarioDTO> Crear(UsuarioDTO Model)
        {
            try
            {
                if(await Obtener(Model.Cedula) != null) throw new TaskCanceledException("Ya existe un usuario con esa cédula");
                else
                {
                    var DbUsuario = _mapper.Map<Usuario>(Model);
                    var DbPersona = _mapper.Map<Persona>(Model.Persona);
                    DbUsuario.Clave = Ferramentas.ConvertToSha256(DbUsuario.Clave);

                    var rspModel = await _UsuarioRepository.Registrar(DbUsuario, DbPersona);

                    if (rspModel)
                    {
                        return await Obtener(Model.Cedula);
                    }
                    else throw new TaskCanceledException("No se pudo crear");
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var consulta = _modelRepository.Consultar(p => p.Cedula == id);
                var fromDBmodel = await consulta.FirstOrDefaultAsync();

                if (fromDBmodel != null)
                {
                    var respuesta = await _modelRepository.Eliminar(fromDBmodel);

                    if (!respuesta) throw new TaskCanceledException("No se pudo eliminar");
                    else return respuesta;
                }
                else throw new TaskCanceledException("No se encontraron coincidencias");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UsuarioDTO>> Lista(int Rol, string buscar)
        {
            try
            {
                IQueryable<Usuario>? consulta;

                if(Rol != 0)
                {
                    consulta = _modelRepository.Consultar(p => p.IdRol == Rol && string.Concat(p.Cedula.ToString(), p.NombreUsuario.ToLower(), p.Correo.ToLower()).Contains(buscar.ToLower()));
                }
                else
                {
                    consulta = _modelRepository.Consultar(p => string.Concat(p.Cedula.ToString(), p.NombreUsuario.ToLower(), p.Correo.ToLower()).Contains(buscar.ToLower()));
                }

                List<UsuarioDTO> lista = _mapper.Map<List<UsuarioDTO>>(await consulta.ToListAsync());

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UsuarioDTO> Obtener(int id)
        {
            try
            {
                var consultaU = await _modelRepository.Consultar(p => p.Cedula == id).AsNoTracking().FirstOrDefaultAsync();
                var consultaP = await _PersonaRepository.Consultar(p => p.Cedula == id).AsNoTracking().FirstOrDefaultAsync();

                if (consultaP != null || consultaU != null)
                {
                    var respuesta = _mapper.Map<UsuarioDTO>(consultaU);
                    respuesta.Rol = _mapper.Map<RolDTO>(await _RolRepository.Consultar(r => r.IdRol == consultaU.IdRol).AsNoTracking().FirstOrDefaultAsync());
                    respuesta.Persona = _mapper.Map<PersonaDTO>(consultaP);
                    return respuesta;
                }
                else throw new TaskCanceledException("");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
