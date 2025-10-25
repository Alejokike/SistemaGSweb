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
        private readonly IGenericoRepository<Rol> _modelRepositoryRol;
        private readonly IGenericoRepository<Persona> _modelRepositoryPersona;
        private readonly IMapper _mapper;
        public UsuarioService(IGenericoRepository<Usuario> modelRepository, IGenericoRepository<Rol> modelRepositoryRol, IGenericoRepository<Persona> modelRepositoryPersona, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _modelRepositoryRol = modelRepositoryRol;
            _modelRepositoryPersona = modelRepositoryPersona;
            _mapper = mapper;
        }

        public async Task<SesionDTO> Autorizacion(LoginDTO Model)
        {
            try
            {
                var consulta = _modelRepository.Consultar(p => (p.Correo == Model.Correo || p.NombreUsuario == Model.Correo) && p.Clave == Ferramentas.ConvertToSha256(Model.Clave) && p.Activo == true);
                var fromDBmodel = await consulta.FirstOrDefaultAsync();

                if (fromDBmodel != null) 
                {
                    return _mapper.Map<SesionDTO>(fromDBmodel);
                    /*
                        IdUsuario = fromDBmodel.IdUsuario,
                        Correo = fromDBmodel.Correo,
                        NombreUsuario = fromDBmodel.NombreUsuario,
                        RolUsuario = _mapper.Map<RolDTO>(RolUser),
                        Perfil = _mapper.Map<PersonaDTO>(PerfilUser)
                    */
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
                var DbModel = _mapper.Map<Usuario>(Model);
                DbModel.Clave = Ferramentas.ConvertToSha256(DbModel.Clave);
                DbModel.IdRol = Model.IdRol;
                DbModel.Cedula = Model.Cedula;
                var rspModel = await _modelRepository.Crear(DbModel);

                if (rspModel.IdUsuario != 0) return _mapper.Map<UsuarioDTO>(rspModel);
                else throw new TaskCanceledException("No se pudo crear");
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
                var consulta = _modelRepository.Consultar(p => p.IdUsuario == Model.IdUsuario);
                var fromDBmodel = await consulta.FirstOrDefaultAsync();

                if (fromDBmodel != null)
                {
                    fromDBmodel.NombreUsuario = Model.NombreUsuario;
                    fromDBmodel.Cedula = Model.Cedula;
                    fromDBmodel.Correo = Model.Correo;
                    fromDBmodel.Clave = Ferramentas.ConvertToSha256(Model.Clave);
                    fromDBmodel.IdRol = Model.IdRol;
                    fromDBmodel.Activo = Model.Activo;
                    fromDBmodel.ResetearClave = false;
                    var respuesta = await _modelRepository.Editar(fromDBmodel);

                    if (!respuesta) throw new TaskCanceledException("No se pudo editar");
                    else return respuesta;
                }
                else throw new TaskCanceledException("No se encontraron coincidencias");
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
                var consulta = _modelRepository.Consultar(p => p.IdUsuario == id);
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
                    consulta = _modelRepository.Consultar(p => p.IdRol == Rol && string.Concat(p.Cedula.ToString(), p.NombreUsuario.ToLower(), p.NombreCompleto.ToLower(), p.Correo.ToLower()).Contains(buscar.ToLower()));
                }
                else
                {
                    consulta = _modelRepository.Consultar(p => string.Concat(p.Cedula.ToString(), p.NombreUsuario.ToLower(), p.NombreCompleto.ToLower(), p.Correo.ToLower()).Contains(buscar.ToLower()));
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
                var consulta = _modelRepository.Consultar(p => p.IdUsuario == id);
                var fromDBmodel = await consulta.FirstOrDefaultAsync();

                if (fromDBmodel != null)
                {
                    return _mapper.Map<UsuarioDTO>(fromDBmodel);
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
