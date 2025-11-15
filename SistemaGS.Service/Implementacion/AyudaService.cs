using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.Model;
using SistemaGS.Repository.Contrato;
using SistemaGS.Service.Contrato;

namespace SistemaGS.Service.Implementacion
{
    public class AyudaService : IAyudaService
    {
        private readonly IGenericoRepository<Ayuda> _modelRepository;
        private readonly IAyudaRepository _AyudaRepository;
        private readonly IGenericoRepository<Persona> _personaRepository;

        private readonly IMapper _mapper;
        public AyudaService(IGenericoRepository<Ayuda> modelRepository, IAyudaRepository AyudaRepository, IGenericoRepository<Persona> personaRepository,IMapper mapper)
        {
            _modelRepository = modelRepository;
            _AyudaRepository = AyudaRepository;
            _personaRepository = personaRepository;
            _mapper = mapper;
        }

        public async Task<bool> CambiarEstado(string estado, int idAyuda)
        {
            return await _AyudaRepository.CambiarEstado(estado, idAyuda);
        }

        public async Task<AyudaDTO> Crear(AyudaDTO Model)
        {
            try
            {
                if (
                    !await _personaRepository.Consultar(s => s.Cedula == Model.Solicitante).AnyAsync() ||
                    !await _personaRepository.Consultar(f => f.Cedula == Model.Funcionario).AnyAsync()
                    )
                    throw new TaskCanceledException("Las personas en la ayuda no existen en sistema");
                if(Model.Solicitante != Model.Funcionario) throw new TaskCanceledException("Un solicitante no puede gestionar su propia ayuda");

                var responseDB = await _modelRepository.Crear(_mapper.Map<Ayuda>(Model));
                if (responseDB != null) return _mapper.Map<AyudaDTO>(responseDB);
                else throw new TaskCanceledException("No se pudo crear");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<bool> Editar(AyudaDTO Model)
        {
            try
            {
                if (
                    !await _personaRepository.Consultar(s => s.Cedula == Model.Solicitante).AnyAsync() ||
                    !await _personaRepository.Consultar(f => f.Cedula == Model.Funcionario).AnyAsync()
                    )
                    throw new TaskCanceledException("Las personas en la ayuda no existen en sistema");
                if (Model.Solicitante == Model.Funcionario) throw new TaskCanceledException("Un solicitante no puede gestionar su propia ayuda");

                if (await _modelRepository.Consultar(a => a.IdAyuda == Model.IdAyuda).AnyAsync()) return await _modelRepository.Editar(_mapper.Map<Ayuda>(Model));
                else throw new TaskCanceledException("La ayuda seleccionada no existe");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<bool> Eliminar(int idAyuda)
        {
            try
            {
                var model = await _modelRepository.Consultar(a => a.IdAyuda == idAyuda).FirstOrDefaultAsync();

                List<ListaItemDTO> lista = JsonConvert.DeserializeObject<List<ListaItemDTO>>(model!.ListaItems!)!;

                if (lista.Any(l => l.CantidadEntregada > 0)) throw new TaskCanceledException("La ayuda tiene stock asignado, devuelvalo al inventario antes de eliminar");

                if (model != null) return await _modelRepository.Eliminar(model);
                else throw new TaskCanceledException("No existen coincidencias");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<List<AyudaDTO>> Lista(AyudaQuery filtro)
        {
            try
            {
                string query = JsonConvert.SerializeObject(filtro);

                var model = await _AyudaRepository.Listar(query);

                if (model != null) return _mapper.Map<List<AyudaDTO>>(model);
                else throw new TaskCanceledException("No existen coincidencias");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<AyudaDTO> Obtener(int idAyuda)
        {
            try
            {
                var model = await _modelRepository.Consultar(a => a.IdAyuda == idAyuda).FirstOrDefaultAsync();
                if (model != null) return _mapper.Map<AyudaDTO>(model);
                else throw new TaskCanceledException("No existen coincidencias");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }               
    }
}
