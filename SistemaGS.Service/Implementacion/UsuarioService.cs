﻿using Microsoft.EntityFrameworkCore;
using SistemaGS.Model;
using SistemaGS.DTO;
using SistemaGS.Repository.Contrato;
using SistemaGS.Service.Contrato;
using AutoMapper;

namespace SistemaGS.Service.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericoRepository<Usuario> _modelRepository;
        private readonly IGenericoRepository<Rol> _modelRepositoryRol;
        private readonly IMapper _mapper;
        public UsuarioService(IGenericoRepository<Usuario> modelRepository, IGenericoRepository<Rol> modelRepositoryRol, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _modelRepositoryRol = modelRepositoryRol;
            _mapper = mapper;
        }

        public async Task<SesionDTO> Autorizacion(LoginDTO Model)
        {
            try
            {
                var consulta = _modelRepository.Consultar(p => p.Correo == Model.Correo && p.Clave == Model.Clave);
                var fromDBmodel = await consulta.FirstOrDefaultAsync();

                if (fromDBmodel != null) 
                {
                    var rolUser = _modelRepositoryRol.Consultar(r => r.IdRol == fromDBmodel.IdRol);
                    var RolUser = await rolUser.FirstOrDefaultAsync();

                    return new SesionDTO
                    {
                        IdUsuario = fromDBmodel.IdUsuario,
                        NombreCompleto = fromDBmodel.NombreCompleto,
                        Correo = fromDBmodel.Correo,
                        NombreUsuario = fromDBmodel.NombreUsuario,
                        IdRol = fromDBmodel.IdRol,
                        Rol = RolUser!.Nombre
                    };
                } //return _mapper.Map<SesionDTO>(fromDBmodel);
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
                    fromDBmodel.NombreCompleto = Model.NombreCompleto;
                    fromDBmodel.Correo = Model.Correo;
                    fromDBmodel.Clave = Model.Clave;
                    fromDBmodel.IdRol = Model.IdRol;
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
                var consulta = _modelRepository.Consultar(p =>
                p.IdRol == Rol &&
                string.Concat(p.NombreCompleto.ToLower(), p.NombreUsuario.ToLower(),p.Correo.ToLower()).Contains(buscar.ToLower())
                );

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

                if (fromDBmodel != null) return _mapper.Map<UsuarioDTO>(fromDBmodel);
                else throw new TaskCanceledException("");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
