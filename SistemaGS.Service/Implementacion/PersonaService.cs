using Microsoft.EntityFrameworkCore;
using SistemaGS.Model;
using SistemaGS.DTO;
using SistemaGS.Repository.Contrato;
using SistemaGS.Service.Contrato;
using AutoMapper;

namespace SistemaGS.Service.Implementacion
{
    public class PersonaService : IPersonaService
    {
        private readonly IGenericoRepository<Persona> _modelRepository;
        private readonly IMapper _mapper;
        public PersonaService(IGenericoRepository<Persona> modelRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }
        public async Task<PersonaDTO> Crear(PersonaDTO Model)
        {
            try
            {
                var DbModel = _mapper.Map<Persona>(Model);
                var rspModel = await _modelRepository.Crear(DbModel);

                if (rspModel.Cedula != 0) return _mapper.Map<PersonaDTO>(rspModel);
                else throw new TaskCanceledException("No se pudo crear");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Editar(PersonaDTO Model, int Cedula)
        {
            try
            {
                var consulta = _modelRepository.Consultar(p => p.Cedula == Cedula);
                var fromDBmodel = await consulta.FirstOrDefaultAsync();

                if (fromDBmodel != null)
                {
                    fromDBmodel = _mapper.Map<Persona>(Model);
                    await Eliminar(Cedula);
                    var respuesta = (await Crear(Model)).Cedula != 0;
                    //var respuesta = await _modelRepository.Editar(fromDBmodel);

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

        public async Task<List<PersonaDTO>> Lista(int tipo, string buscar)
        {
            try
            {
                IQueryable<Persona>? consulta = _modelRepository.Consultar(p => string.Concat(p.Nombre!.ToLower(), p.Apellido!.ToLower(), p.Cedula.ToString()).Contains(buscar.ToLower()));

                List<PersonaDTO> lista = _mapper.Map<List<PersonaDTO>>(await consulta.ToListAsync());

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PersonaDTO> Obtener(int id)
        {
            try
            {
                var consulta = _modelRepository.Consultar(p => p.Cedula == id);
                var fromDBmodel = await consulta.FirstOrDefaultAsync();

                if (fromDBmodel != null)
                {
                    return _mapper.Map<PersonaDTO>(fromDBmodel);
                }
                else throw new TaskCanceledException("Esta persona no existe");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
